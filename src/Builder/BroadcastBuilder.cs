using System.Collections.Generic;
using System.Linq;

namespace Ntreev.AspNetCore.WebSocketIo.Builder
{
    /// <inheritdoc cref="IBroadcastBuilder"/>
    public class BroadcastBuilder : IBroadcastBuilder
    {
        private readonly IWebSocketIo _webSocketIo;
        private readonly IWebSocketIoConnectionManager _webSocketIoConnectionManager;

        public BroadcastBuilder(IWebSocketIo webSocketIo,
            IWebSocketIoConnectionManager webSocketIoConnectionManager)
        {
            _webSocketIo = webSocketIo;
            _webSocketIoConnectionManager = webSocketIoConnectionManager;
        }

        /// <inheritdoc cref="IBroadcastBuilder.To"/>
        public IEnumerable<IWebSocketIo> To(string roomKey)
        {
            return _webSocketIoConnectionManager.GetClientsInRoom(roomKey);
        }

        /// <inheritdoc cref="IBroadcastBuilder.In"/>
        public IEnumerable<IWebSocketIo> In(string roomKey)
        {
            return To(roomKey).Where(webSocketIo => webSocketIo != _webSocketIo);
        }
    }
}