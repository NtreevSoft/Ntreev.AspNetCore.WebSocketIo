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

        /// <inheritdoc cref="IBroadcastBuilder.To()"/>
        public IEnumerable<IWebSocketIo> To()
        {
            return In().Where(webSocketIo => webSocketIo != _webSocketIo);
        }

        /// <inheritdoc cref="IBroadcastBuilder.To(string)"/>
        public IEnumerable<IWebSocketIo> To(string channelKey)
        {
            return In(channelKey).Where(webSocketIo => webSocketIo != _webSocketIo);
        }

        /// <inheritdoc cref="IBroadcastBuilder.In()"/>
        public IEnumerable<IWebSocketIo> In()
        {
            return _webSocketIoConnectionManager.GetAll();
        }

        /// <inheritdoc cref="IBroadcastBuilder.In(string)"/>
        public IEnumerable<IWebSocketIo> In(string channelKey)
        {
            return _webSocketIoConnectionManager.GetClientsInChannel(channelKey);
        }
    }
}