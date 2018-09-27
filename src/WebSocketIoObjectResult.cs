using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo
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
            return _webSocketIo.Socket.SendDataAsync(Value.ToJson());
        }
    }
}