using System.Collections.Generic;
using System.Linq;

namespace Ntreev.AspNetCore.WebSocketIo.Builder
{
    public interface IBroadcastBuilder
    {
        IEnumerable<IWebSocketIo> To(string roomKey);
        IEnumerable<IWebSocketIo> In(string roomKey);
    }

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

        public IEnumerable<IWebSocketIo> To(string roomKey)
        {
            return _webSocketIoConnectionManager.GetClientsInRoom(roomKey);
        }

        public IEnumerable<IWebSocketIo> In(string roomKey)
        {
            return To(roomKey).Where(webSocketIo => webSocketIo != _webSocketIo);
        }
    }
}
