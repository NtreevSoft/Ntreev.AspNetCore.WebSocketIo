using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Ntreev.AspNetCore.WebSocketIo.Builder;
using Ntreev.AspNetCore.WebSocketIo.Extensions;
using Ntreev.AspNetCore.WebSocketIo.Mvc;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// 웹소켓 기능을 제공하는 클래스 입니다.
    /// </summary>
    public class WebSocketIo : IWebSocketIo
    {
        private readonly IWebSocketIoConnectionManager _webSocketIoConnectionManager;
        public Guid SocketId { get; }
        public WebSocket Socket { get; }
        public IBroadcastBuilder Broadcast { get; }
        public IPrivateBuilder Private { get; }
        public IList<string> JoinedChannels { get; }

        public event EventHandler<WebSocketIoEventArgs> Leaved;
        public event EventHandler Disconnecting;
        
        public WebSocketIo(Guid socketId, 
            WebSocket socket, 
            IWebSocketIoConnectionManager webSocketIoConnectionManager)
        {
            SocketId = socketId;
            Socket = socket;
            JoinedChannels = new List<string>();
            _webSocketIoConnectionManager = webSocketIoConnectionManager;
            Broadcast = new BroadcastBuilder(this, webSocketIoConnectionManager);
            Private = new PrivateBuilder(webSocketIoConnectionManager);
        }

        public WebSocketIo(Guid socketId, 
            WebSocket socket, 
            IEnumerable<string> joinedChannels, 
            IWebSocketIoConnectionManager webSocketIoConnectionManager) : this(socketId, socket, webSocketIoConnectionManager)
        {
            JoinedChannels = new List<string>(joinedChannels);
        }

        /// <inheritdoc cref="OnLeaved"/>
        public void OnLeaved(object sender, WebSocketIoEventArgs args)
        {
            Leaved?.Invoke(sender, args);
        }

        /// <inheritdoc cref="OnDisconnecting"/>
        public void OnDisconnecting(object sender)
        {
            Disconnecting?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc cref="SendDataAsync(string,bool,CancellationToken)"/>
        public Task SendDataAsync(string data, bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Socket.SendDataAsync(data, endOfMessage, cancellationToken);
        }

        /// <inheritdoc cref="SendDataAsync(object,bool,CancellationToken)"/>
        public Task SendDataAsync(object obj, bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Socket.SendDataAsync(obj.ToJson(), endOfMessage, cancellationToken);
        }

        /// <inheritdoc cref="SendDataAsync(WebSocketIoResponse, CancellationToken)"/>
        public Task SendDataAsync(WebSocketIoResponse response, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Socket.SendDataAsync(response.ToJson(), true, cancellationToken);
        }

        /// <inheritdoc cref="JoinAsync"/>
        public Task JoinAsync(string channelKey)
        {
            return _webSocketIoConnectionManager.JoinAsync(channelKey, this);
        }

        /// <inheritdoc cref="LeaveAsync"/>
        public Task LeaveAsync(string channelKey)
        {
            return _webSocketIoConnectionManager.LeaveAsync(channelKey, this);
        }

        /// <inheritdoc cref="LeaveAllAsync"/>
        public Task LeaveAllAsync()
        {
            return _webSocketIoConnectionManager.LeaveAllAsync(this);
        }
    }
}