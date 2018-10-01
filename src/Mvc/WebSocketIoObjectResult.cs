using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    public class WebSocketIoObjectResult : ObjectResult
    {
        private readonly IWebSocketIo _webSocketIo;

        public WebSocketIoObjectResult(IWebSocketIo webSocketIo, 
            object value) : base(value)
        {
            _webSocketIo = webSocketIo;
        }

        public override void ExecuteResult(ActionContext context)
        {
            ExecuteResultAsync(context).GetAwaiter();
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var packet = context.HttpContext.Items["web-socket-io-packet"] as WebSocketIoPacket;
            if (packet == null)
                throw new NullReferenceException(nameof(packet));

            return _webSocketIo.Socket.SendDataAsync(new WebSocketIoResponse(packet.Id, Value).ToJson());
        }
    }
}