using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public interface IWebSocketIoConnectionManager
    {
        void Add(Guid guid, IWebSocketIo socket);
        IWebSocketIo GetOrDefault(Guid guid);
        Task RemoveAsync(Guid guid);
        Task JoinAsync(string key, IWebSocketIo webSocketIo);
        Task LeaveAsync(string key, IWebSocketIo webSocketIo);
        Task LeaveAllAsync(IWebSocketIo webSocketIo);
        Task DisposeAsync(IWebSocketIo webSocketIo);
        IEnumerable<IWebSocketIo> GetClientsInRoom(string key);
        IEnumerable<IWebSocketIo> GetAll();

        event EventHandler<IWebSocketIo> Connected;
        event EventHandler<IWebSocketIo> Disconnected;

        void OnConnected(object sender, IWebSocketIo webSocketIo);
        void OnDisconnected(object sender, IWebSocketIo webSocketIo);
    }
}