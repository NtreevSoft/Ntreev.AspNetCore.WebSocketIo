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
        private readonly IDictionary<string, IList<IWebSocketIo>> _rooms = new ConcurrentDictionary<string, IList<IWebSocketIo>>();

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
        public async Task JoinAsync(string key, IWebSocketIo webSocketIo)
        {
            await Task.Run(() =>
            {
                try
                {
                    _semaphoreSlim.Wait();
                    if (!_rooms.ContainsKey(key))
                    {
                        _rooms[key] = new List<IWebSocketIo>();
                    }

                    if (!_rooms[key].Contains(webSocketIo))
                    {
                        _rooms[key].Add(webSocketIo);
                        webSocketIo.JoinedRooms.Add(key);
                    }
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });
        }

        /// <inheritdoc cref="LeaveAsync"/>
        public async Task LeaveAsync(string key, IWebSocketIo webSocketIo)
        {
            await Task.Run(() =>
            {
                try
                {
                    _semaphoreSlim.Wait();

                    if (_rooms[key].Contains(webSocketIo))
                    {
                        _rooms[key].Remove(webSocketIo);
                        webSocketIo.JoinedRooms.Remove(key);
                        webSocketIo.OnLeaved(this, new WebSocketIoEventArgs(key, webSocketIo));
                    }
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });
        }

        /// <inheritdoc cref="LeaveAllAsync"/>
        public async Task LeaveAllAsync(IWebSocketIo webSocketIo)
        {
            await Task.Run(() =>
            {
                try
                {
                    _semaphoreSlim.Wait();

                    foreach (var room in webSocketIo.JoinedRooms)
                    {
                        if (_rooms.ContainsKey(room))
                        {
                            _rooms[room].Remove(webSocketIo);
                            webSocketIo.OnLeaved(this, new WebSocketIoEventArgs(room, webSocketIo));
                        }
                    }

                    webSocketIo.JoinedRooms.Clear();
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });
        }

        /// <inheritdoc cref="DisposeAsync"/>
        public async Task DisposeAsync(IWebSocketIo webSocketIo)
        {
            await Task.Run(async () =>
            {
                try
                {
                    await _semaphoreSlim.WaitAsync();

                    if (_webSocketIos.ContainsKey(webSocketIo.SocketId))
                    {
                        _webSocketIos.Remove(webSocketIo.SocketId);
                    }

                    foreach (var room in webSocketIo.JoinedRooms)
                    {
                        if (_rooms.ContainsKey(room))
                        {
                            _rooms[room].Remove(webSocketIo);
                        }
                    }

                    webSocketIo.JoinedRooms.Clear();

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

        /// <inheritdoc cref="GetClientsInRoom"/>
        public IEnumerable<IWebSocketIo> GetClientsInRoom(string key)
        {
            if (_rooms.ContainsKey(key)) return _rooms[key].AsEnumerable();

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