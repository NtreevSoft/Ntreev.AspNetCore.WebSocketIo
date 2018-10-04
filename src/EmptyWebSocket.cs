using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// �� ������ Ŭ���� �Դϴ�.
    /// <remarks>
    /// HTTP ��û�� ��츸 �� <see cref="EmptyWebSocket"/> ��ü�� �����ؼ� ����մϴ�. HTTP �� ��� WebSocketIo ���� API ȣȯ���� ���� ����ϴ� Ŭ���� �Դϴ�.
    /// </remarks>
    /// </summary>
    public class EmptyWebSocket : WebSocket
    {
        /// <inheritdoc cref="Abort"/>
        public override void Abort()
        {
        }

        /// <inheritdoc cref="CloseAsync"/>
        public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="CloseOutputAsync"/>
        public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="Dispose"/>
        public override void Dispose()
        {
        }

        /// <inheritdoc cref="ReceiveAsync"/>
        public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            return new Task<WebSocketReceiveResult>(o => null, cancellationToken);
        }

        /// <inheritdoc cref="SendAsync"/>
        public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="CloseStatus"/>
        public override WebSocketCloseStatus? CloseStatus => WebSocketCloseStatus.Empty;

        /// <inheritdoc cref="CloseStatusDescription"/>
        public override string CloseStatusDescription => string.Empty;

        /// <inheritdoc cref="State"/>
        public override WebSocketState State => WebSocketState.None;

        /// <inheritdoc cref="SubProtocol"/>
        public override string SubProtocol => null;
    }
}