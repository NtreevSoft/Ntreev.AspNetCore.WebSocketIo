using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    public class WebSocketIoOkEventObjectResult : WebSocketIoOkObjectResult
    {
        private readonly IWebSocketIo _webSocketIo;
        private readonly string _emitName;

        public WebSocketIoOkEventObjectResult(IWebSocketIo webSocketIo, string emitName, object value) : base(webSocketIo, value)
        {
            _webSocketIo = webSocketIo;
            _emitName = emitName;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var packet = context.HttpContext.Items["web-socket-io-packet"] as WebSocketIoPacket;
            if (packet == null)
                throw new NullReferenceException(nameof(packet));

            return _webSocketIo.Socket.SendDataAsync(new WebSocketIoResponse(packet.Id, WebSocketIoResponseType.Event, _emitName, StatusCode, Value).ToJson());
        }
    }
}