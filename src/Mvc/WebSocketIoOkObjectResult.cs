using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    /// <summary>
    /// Ok ���� �ڵ�� �Բ� ��� ��ü�� ��Ÿ���� Ŭ���� �Դϴ�.
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