using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public class EmptyWebSocket : WebSocket
    {
        public override void Abort()
        {
        }

        public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
        }

        public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            return new Task<WebSocketReceiveResult>(o => null, cancellationToken);
        }

        public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override WebSocketCloseStatus? CloseStatus => WebSocketCloseStatus.Empty;
        public override string CloseStatusDescription => string.Empty;
        public override WebSocketState State => WebSocketState.None;
        public override string SubProtocol => null;
    }
}