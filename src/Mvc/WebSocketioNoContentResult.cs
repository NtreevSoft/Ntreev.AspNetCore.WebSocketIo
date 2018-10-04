using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    /// <summary>
    /// 콘텐트 결과가 없는 결과를 나타내는 클래스 입니다.
    /// </summary>
    public class WebSocketIoNoContentResult : NoContentResult
    {
        private readonly IWebSocketIo _webSocketIo;

        public WebSocketIoNoContentResult(IWebSocketIo webSocketIo)
        {
            _webSocketIo = webSocketIo;
        }

        /// <inheritdoc cref="ExecuteResult"/>
        public override void ExecuteResult(ActionContext context)
        {
        }

        /// <inheritdoc cref="ExecuteResultAsync"/>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            var packet = context.HttpContext.Items["web-socket-io-packet"] as WebSocketIoPacket;
            if (packet == null)
                throw new NullReferenceException(nameof(packet));

            return _webSocketIo.SendDataAsync(new WebSocketIoResponse(packet.Id, 204, new { }).ToJson());
        }
    }
}
