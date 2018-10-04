using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    /// <summary>
    /// 웹소켓의 콘텐트 결과를 나타내는 클래스 입니다.
    /// </summary>
    public class WebSocketIoContentResult : ContentResult
    {
        private readonly IWebSocketIo _webSocketIo;
        private readonly string _content;

        public WebSocketIoContentResult(IWebSocketIo webSocketIo, string content)
        {
            _webSocketIo = webSocketIo;
            _content = content;
        }

        /// <inheritdoc cref="ExecuteResult"/>
        public override void ExecuteResult(ActionContext context)
        {
            ExecuteResultAsync(context).GetAwaiter().GetResult();
        }

        /// <inheritdoc cref="ExecuteResultAsync"/>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            var packet = context.HttpContext.Items["web-socket-io-packet"] as WebSocketIoPacket;
            if (packet == null)
                throw new NullReferenceException(nameof(packet));

            return _webSocketIo.Socket.SendDataAsync(new WebSocketIoResponse(packet.Id, Content).ToJson());
        }
    }
}
