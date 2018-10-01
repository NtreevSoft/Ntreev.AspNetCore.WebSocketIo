using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    public class WebSocketIoNoContentResult : NoContentResult
    {
        private readonly IWebSocketIo _webSocketIo;

        public WebSocketIoNoContentResult(IWebSocketIo webSocketIo)
        {
            _webSocketIo = webSocketIo;
        }

        public override void ExecuteResult(ActionContext context)
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var packet = context.HttpContext.Items["web-socket-io-packet"] as WebSocketIoPacket;
            if (packet == null)
                throw new NullReferenceException(nameof(packet));

            return _webSocketIo.SendDataAsync(new WebSocketIoResponse(packet.Id, 204, new { }).ToJson());
        }
    }
}
