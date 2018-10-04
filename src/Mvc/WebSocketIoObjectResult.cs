using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    /// <summary>
    /// 객체를 직렬화하여 나타내는 클래스 입니다.
    /// </summary>
    public class WebSocketIoObjectResult : ObjectResult
    {
        private readonly IWebSocketIo _webSocketIo;

        public WebSocketIoObjectResult(IWebSocketIo webSocketIo, 
            object value) : base(value)
        {
            _webSocketIo = webSocketIo;
        }

        /// <inheritdoc cref="ExecuteResult"/>
        public override void ExecuteResult(ActionContext context)
        {
            ExecuteResultAsync(context).GetAwaiter();
        }

        /// <inheritdoc cref="ExecuteResultAsync"/>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            var packet = context.HttpContext.Items["web-socket-io-packet"] as WebSocketIoPacket;
            if (packet == null)
                throw new NullReferenceException(nameof(packet));

            return _webSocketIo.Socket.SendDataAsync(new WebSocketIoResponse(packet.Id, Value).ToJson());
        }
    }
}