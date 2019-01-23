using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Ntreev.AspNetCore.WebSocketIo.Mvc;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// 웹소켓을 사용하는 컨트롤러에서 필요한 클래스 입니다.
    /// <remarks>
    /// 이 컨트롤러를 상속하면 HTTP/웹소켓 모두에서 노출된 API 를 호출할 수 있습니다.
    /// </remarks>
    /// </summary>
    public class WebSocketIoController : Controller
    {
        private readonly IWebSocketIo _webSocketIo;

        public WebSocketIoController(IWebSocketIo webSocketIo)
        {
            _webSocketIo = webSocketIo;
        }
        
        /// <inheritdoc cref="Ok()"/>
        public override OkResult Ok()
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoOkResult(_webSocketIo)
                : base.Ok();
        }

        /// <inheritdoc cref="Ok(object)"/>
        public override OkObjectResult Ok(object value)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoOkObjectResult(_webSocketIo, value)
                : base.Ok(value);
        }

        /// <summary>
        /// 서버에서 클라이언트로 이벤트를 발생합니다.
        /// </summary>
        /// <param name="emitName">이벤트 이름 입니다.</param>
        /// <param name="value">이벤트 값 입니다.</param>
        /// <returns></returns>
        public virtual OkObjectResult OkEvent(string emitName, object value)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoOkEventObjectResult(_webSocketIo, emitName, value)
                : base.Ok(value);
        }

        /// <inheritdoc cref="NoContent"/>
        public override NoContentResult NoContent()
        {
            return HttpContext.WebSockets.IsWebSocketRequest 
                ? new WebSocketIoNoContentResult(_webSocketIo) 
                : base.NoContent();
        }

        /// <inheritdoc cref="StatusCode(int)"/>
        public override StatusCodeResult StatusCode(int statusCode)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoStatusCodeResult()
                : base.StatusCode(statusCode);
        }

        /// <inheritdoc cref="StatusCode(int, object)"/>
        public override ObjectResult StatusCode(int statusCode, object value)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoObjectResult(_webSocketIo, value) 
                : base.StatusCode(statusCode, value);
        }

        /// <inheritdoc cref="Content(string)"/>
        public override ContentResult Content(string content)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoContentResult(_webSocketIo, content)
                : base.Content(content);
        }

        /// <inheritdoc cref="Content(string, string)"/>
        public override ContentResult Content(string content, string contentType)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoContentResult(_webSocketIo, content)
                : base.Content(content, contentType);
        }

        /// <inheritdoc cref="Content(string, string, Encoding)"/>
        public override ContentResult Content(string content, string contentType, Encoding contentEncoding)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoContentResult(_webSocketIo, content)
                : base.Content(content, contentType, contentEncoding);
        }

        /// <inheritdoc cref="Content(string, MediaTypeHeaderValue)"/>
        public override ContentResult Content(string content, MediaTypeHeaderValue contentType)
        {
            return HttpContext.WebSockets.IsWebSocketRequest
                ? new WebSocketIoContentResult(_webSocketIo, content)
                : base.Content(content, contentType);
        }
    }
}