using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    public class WebSocketIoStatusCodeResult : StatusCodeResult
    {
        private readonly IWebSocketIo _webSocketIo;

        public WebSocketIoStatusCodeResult(IWebSocketIo webSocketIo, int statusCode) : base(statusCode)
        {
            _webSocketIo = webSocketIo;
        }

        public WebSocketIoStatusCodeResult() : base(200)
        {
        }

        public override void ExecuteResult(ActionContext context)
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var packet = context.HttpContext.Items["web-socket-io-packet"] as WebSocketIoPacket;
            if (packet == null)
                throw new NullReferenceException(nameof(packet));

            return _webSocketIo.SendDataAsync(new WebSocketIoResponse(packet.Id, new { }).ToJson());
        }
    }
}
