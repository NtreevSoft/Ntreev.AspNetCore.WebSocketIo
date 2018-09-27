using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketIoConnectionManager : IWebSocketIoConnectionManager
    {
        private readonly IDictionary<Guid, IWebSocketIo> _webSocketIos = new ConcurrentDictionary<Guid, IWebSocketIo>();
        private readonly IDictionary<string, IList<IWebSocketIo>> _rooms = new ConcurrentDictionary<string, IList<IWebSocketIo>>();

        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public void Add(Guid guid, IWebSocketIo socket)
        {
            _webSocketIos.Add(new KeyValuePair<Guid, IWebSocketIo>(guid, socket));
        }

        public IWebSocketIo GetOrDefault(Guid guid)
        {
            var result = _webSocketIos.TryGetValue(guid, out var socket);
            return result == false ? null : socket;
        }

        public Task RemoveAsync(Guid guid)
        {
            var socket = GetOrDefault(guid);
            if (socket != null)
            {
                _webSocketIos.Remove(guid);
            }

            return Task.CompletedTask;
        }

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

                    await webSocketIo.Socket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "",
                        CancellationToken.None);
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });
        }

        public IEnumerable<IWebSocketIo> GetAll()
        {
            return _webSocketIos.Values;
        }

        public IEnumerable<IWebSocketIo> GetClientsInRoom(string key)
        {
            if (_rooms.ContainsKey(key)) return _rooms[key].AsEnumerable();

            return Enumerable.Empty<IWebSocketIo>();
        }
        
        public event EventHandler<IWebSocketIo> Connected;
        public event EventHandler<IWebSocketIo> Disconnected;
        
        public virtual void OnConnected(object sender, IWebSocketIo webSocketIo)
        {
            Connected?.Invoke(sender, webSocketIo);
        }

        public virtual void OnDisconnected(object sender, IWebSocketIo webSocketIo)
        {
            Disconnected?.Invoke(sender, webSocketIo);
        }
    }
}