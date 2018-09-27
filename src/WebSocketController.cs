using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketController : Controller
    {
        private readonly IWebSocketIo _webSocketIo;

        public WebSocketController(IWebSocketIo webSocketIo)
        {
            _webSocketIo = webSocketIo;
        }
        
        public override OkResult Ok()
        {
            return new WebSocketIoOkResult();
        }

        public override OkObjectResult Ok(object value)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoOkObjectResult(_webSocketIo, value)
                : base.Ok(value);
        }

        public override NoContentResult NoContent()
        {
            return HttpContext.WebSockets.IsWebSocketRequest 
                ? new WebSocketioNoContentResult() 
                : base.NoContent();
        }

        public override StatusCodeResult StatusCode(int statusCode)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoStatusCodeResult()
                : base.StatusCode(statusCode);
        }

        public override ObjectResult StatusCode(int statusCode, object value)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoObjectResult(_webSocketIo, value) 
                : base.StatusCode(statusCode, value);
        }

        public override ContentResult Content(string content)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoContentResult(_webSocketIo, content)
                : base.Content(content);
        }

        public override ContentResult Content(string content, string contentType)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoContentResult(_webSocketIo, content)
                : base.Content(content, contentType);
        }

        public override ContentResult Content(string content, string contentType, Encoding contentEncoding)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoContentResult(_webSocketIo, content)
                : base.Content(content, contentType, contentEncoding);
        }

        public override ContentResult Content(string content, MediaTypeHeaderValue contentType)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoContentResult(_webSocketIo, content)
                : base.Content(content, contentType);
        }
    }
}