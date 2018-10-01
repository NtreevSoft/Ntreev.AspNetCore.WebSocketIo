using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    public class WebSocketIoContentResult : ContentResult
    {
        private readonly IWebSocketIo _webSocketIo;
        private readonly string _content;

        public WebSocketIoContentResult(IWebSocketIo webSocketIo, string content)
        {
            _webSocketIo = webSocketIo;
            _content = content;
        }

        public override void ExecuteResult(ActionContext context)
        {
            ExecuteResultAsync(context).GetAwaiter().GetResult();
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var packet = context.HttpContext.Items["web-socket-io-packet"] as WebSocketIoPacket;
            if (packet == null)
                throw new NullReferenceException(nameof(packet));

            return _webSocketIo.Socket.SendDataAsync(new WebSocketIoResponse(packet.Id, Content).ToJson());
        }
    }
}
