using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    /// <summary>
    /// Ok 상태를 나타내는 클래스 입니다.
    /// </summary>
    public class WebSocketIoOkResult : OkResult
    {
        private readonly IWebSocketIo _webSocketIo;

        public WebSocketIoOkResult(IWebSocketIo webSocketIo)
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

            return _webSocketIo.SendDataAsync(new WebSocketIoResponse(packet.Id, new { }).ToJson());
        }
    }
}