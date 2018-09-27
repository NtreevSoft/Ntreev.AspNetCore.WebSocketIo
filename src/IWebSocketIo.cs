using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Ntreev.AspNetCore.WebSocketIo.Builder;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public interface IWebSocketIo
    {
        Guid SocketId { get; }
        WebSocket Socket { get; }
        IBroadcastBuilder Broadcast { get; }
        IPrivateBuilder Private { get; }
        IList<string> JoinedRooms { get; }

        event EventHandler<WebSocketIoEventArgs> Leaved;
        event EventHandler Disconnecting;
        void OnLeaved(object sender, WebSocketIoEventArgs args);
        void OnDisconnecting(object sender);

        Task SendDataAsync(string data, bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken));
        Task SendDataAsync(object obj, bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken));
        Task JoinAsync(string roomKey);
        Task LeaveAsync(string roomKey);
        Task LeaveAllAsync();

    }
}