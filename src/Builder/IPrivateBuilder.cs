using System;

namespace Ntreev.AspNetCore.WebSocketIo.Builder
{
    public interface IPrivateBuilder
    {
        IWebSocketIo To(Guid socketId);
    }

    public class PrivateBuilder : IPrivateBuilder
    {
        private readonly IWebSocketIoConnectionManager _webSocketIoConnectionManager;

        public PrivateBuilder(IWebSocketIoConnectionManager webSocketIoConnectionManager)
        {
            _webSocketIoConnectionManager = webSocketIoConnectionManager;
        }

        public IWebSocketIo To(Guid socketId)
        {
            return _webSocketIoConnectionManager.GetOrDefault(socketId);
        }
    }
}
