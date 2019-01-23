using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    /// <summary>
    /// Ok 상태 코드와 함께 결과 객체를 나타내는 클래스 입니다.
    /// </summary>
    public class WebSocketIoOkObjectResult : OkObjectResult
    {
        private readonly IWebSocketIo _webSocketIo;

        public WebSocketIoOkObjectResult(IWebSocketIo webSocketIo,
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