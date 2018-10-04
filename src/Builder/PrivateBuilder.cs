using System;

namespace Ntreev.AspNetCore.WebSocketIo.Builder
{
    /// <inheritdoc cref="IPrivateBuilder"/>
    public class PrivateBuilder : IPrivateBuilder
    {
        private readonly IWebSocketIoConnectionManager _webSocketIoConnectionManager;

        public PrivateBuilder(IWebSocketIoConnectionManager webSocketIoConnectionManager)
        {
            _webSocketIoConnectionManager = webSocketIoConnectionManager;
        }

        /// <inheritdoc cref="IPrivateBuilder.To"/>
        public IWebSocketIo To(Guid socketId)
        {
            var socket = _webSocketIoConnectionManager.GetOrDefault(socketId);
            if (socket == null)
                throw new NullReferenceException(nameof(socket));

            return socket;
        }
    }
}