using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Ntreev.AspNetCore.WebSocketIo.Builder;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketIo : IWebSocketIo
    {
        private readonly IWebSocketIoConnectionManager _webSocketIoConnectionManager;
        public Guid SocketId { get; }
        public WebSocket Socket { get; }
        public IBroadcastBuilder Broadcast { get; }
        public IPrivateBuilder Private { get; }
        public IList<string> JoinedRooms { get; }

        public event EventHandler<WebSocketIoEventArgs> Leaved;
        public event EventHandler Disconnecting;
        
        public WebSocketIo(Guid socketId, 
            WebSocket socket, 
            IWebSocketIoConnectionManager webSocketIoConnectionManager)
        {
            SocketId = socketId;
            Socket = socket;
            JoinedRooms = new List<string>();
            _webSocketIoConnectionManager = webSocketIoConnectionManager;
            Broadcast = new BroadcastBuilder(this, webSocketIoConnectionManager);
            Private = new PrivateBuilder(webSocketIoConnectionManager);
        }

        public WebSocketIo(Guid socketId, 
            WebSocket socket, 
            IEnumerable<string> joinedRooms, 
            IWebSocketIoConnectionManager webSocketIoConnectionManager) : this(socketId, socket, webSocketIoConnectionManager)
        {
            JoinedRooms = new List<string>(joinedRooms);
        }

        public void OnLeaved(object sender, WebSocketIoEventArgs args)
        {
            Leaved?.Invoke(sender, args);
        }

        public void OnDisconnecting(object sender)
        {
            Disconnecting?.Invoke(sender, EventArgs.Empty);
        }

        public Task SendDataAsync(string data, bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Socket.SendDataAsync(data, endOfMessage, cancellationToken);
        }

        public Task SendDataAsync(object obj, bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Socket.SendDataAsync(obj.ToJson(), endOfMessage, cancellationToken);
        }

        public Task JoinAsync(string roomKey)
        {
            return _webSocketIoConnectionManager.JoinAsync(roomKey, this);
        }

        public Task LeaveAsync(string roomKey)
        {
            return _webSocketIoConnectionManager.LeaveAsync(roomKey, this);
        }

        public Task LeaveAllAsync()
        {
            return _webSocketIoConnectionManager.LeaveAllAsync(this);
        }
    }
}