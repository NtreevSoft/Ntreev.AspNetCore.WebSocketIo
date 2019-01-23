using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// 웹소켓을 관리하는 매니저 클래스 입니다.
    /// </summary>
    public class WebSocketIoConnectionManager : IWebSocketIoConnectionManager
    {
        private readonly IDictionary<Guid, IWebSocketIo> _webSocketIos = new ConcurrentDictionary<Guid, IWebSocketIo>();
        private readonly IDictionary<string, IList<IWebSocketIo>> _channels = new ConcurrentDictionary<string, IList<IWebSocketIo>>();

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        /// <inheritdoc cref="Add"/>
        public void Add(Guid guid, IWebSocketIo socket)
        {
            _webSocketIos.Add(new KeyValuePair<Guid, IWebSocketIo>(guid, socket));
        }

        /// <inheritdoc cref="GetOrDefault"/>
        public IWebSocketIo GetOrDefault(Guid guid)
        {
            var result = _webSocketIos.TryGetValue(guid, out var socket);
            return result == false ? null : socket;
        }

        /// <inheritdoc cref="RemoveAsync"/>
        public Task RemoveAsync(Guid guid)
        {
            var socket = GetOrDefault(guid);
            if (socket != null)
            {
                _webSocketIos.Remove(guid);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc cref="JoinAsync"/>
        public Task JoinAsync(string key, IWebSocketIo webSocketIo)
        {
            return Task.Run(() =>
            {
                try
                {
                    _semaphoreSlim.Wait();
                    if (!_channels.ContainsKey(key))
                    {
                        _channels[key] = new List<IWebSocketIo>();
                    }

                    if (!_channels[key].Contains(webSocketIo))
                    {
                        _channels[key].Add(webSocketIo);
                        webSocketIo.JoinedChannels.Add(key);
                    }
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });
        }

        /// <inheritdoc cref="LeaveAsync"/>
        public Task LeaveAsync(string key, IWebSocketIo webSocketIo)
        {
            return Task.Run(() =>
            {
                try
                {
                    _semaphoreSlim.Wait();

                    if (_channels[key].Contains(webSocketIo))
                    {
                        _channels[key].Remove(webSocketIo);
                        webSocketIo.JoinedChannels.Remove(key);
                        webSocketIo.OnLeaved(this, new WebSocketIoEventArgs(key, webSocketIo));
                        
                        if (_channels[key].Count == 0)
                            _channels.Remove(key);
                    }
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });
        }

        /// <inheritdoc cref="LeaveAllAsync"/>
        public Task LeaveAllAsync(IWebSocketIo webSocketIo)
        {
            return Task.Run(() =>
            {
                try
                {
                    _semaphoreSlim.Wait();

                    foreach (var channel in webSocketIo.JoinedChannels)
                    {
                        if (_channels.ContainsKey(channel))
                        {
                            _channels[channel].Remove(webSocketIo);
                            webSocketIo.OnLeaved(this, new WebSocketIoEventArgs(channel, webSocketIo));
                        }
                    }

                    webSocketIo.JoinedChannels.Clear();
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });
        }

        /// <inheritdoc cref="DisposeAsync"/>
        public Task DisposeAsync(IWebSocketIo webSocketIo)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await _semaphoreSlim.WaitAsync();

                    if (_webSocketIos.ContainsKey(webSocketIo.SocketId))
                    {
                        _webSocketIos.Remove(webSocketIo.SocketId);
                    }

                    foreach (var channel in webSocketIo.JoinedChannels)
                    {
                        if (_channels.ContainsKey(channel))
                        {
                            _channels[channel].Remove(webSocketIo);
                        }
                    }

                    webSocketIo.JoinedChannels.Clear();

                    await webSocketIo.Socket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "", CancellationToken.None);
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });
        }

        /// <inheritdoc cref="GetAll"/>
        public IEnumerable<IWebSocketIo> GetAll()
        {
            return _webSocketIos.Values;
        }

        /// <inheritdoc cref="GetClientsInChannel"/>
        public IEnumerable<IWebSocketIo> GetClientsInChannel(string key)
        {
            if (_channels.ContainsKey(key)) return _channels[key].AsEnumerable();

            return Enumerable.Empty<IWebSocketIo>();
        }

        /// <inheritdoc cref="Connected"/>
        public event EventHandler<IWebSocketIo> Connected;

        /// <inheritdoc cref="Disconnected"/>
        public event EventHandler<IWebSocketIo> Disconnected;
        
        /// <inheritdoc cref="OnConnected"/>
        public virtual void OnConnected(object sender, IWebSocketIo webSocketIo)
        {
            Connected?.Invoke(sender, webSocketIo);
        }

        /// <inheritdoc cref="OnDisconnected"/>
        public virtual void OnDisconnected(object sender, IWebSocketIo webSocketIo)
        {
            Disconnected?.Invoke(sender, webSocketIo);
        }
    }
}