using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketIoContentResult : ContentResult
    {
        private readonly IWebSocketIo _webSocketIo;
        private readonly string _content;

        public WebSocketIoContentResult(IWebSocketIo webSocketIo,
            string content)
        {
            _webSocketIo = webSocketIo;
            _content = content;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            return _webSocketIo.Socket.SendDataAsync(_content.ToJson());
        }

        public override void ExecuteResult(ActionContext context)
        {
            ExecuteResultAsync(context).GetAwaiter().GetResult();
        }
    }
}
