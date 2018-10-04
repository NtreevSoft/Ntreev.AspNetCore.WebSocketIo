using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.ObjectPool;

namespace Ntreev.AspNetCore.WebSocketIo.Http
{
    /// <summary>
    /// 웹소켓을 지원하기 위한 <see cref="HttpResponse"/> 클래스 입니다.
    /// <remarks>
    /// 웹소켓의 특성 상 한번의 HTTP 업그레이드를 위해 HTTP 를 요청하고, 소켓이 연결이 된다.
    /// ASP.NET Core 는 Response 가 시작되면 다시 이 Response 를 수정할 수 없고 예외가 발생하게 된다.
    /// 웹소켓 연결인 경우 예외 발생을 방지하기 위해 <see cref="WebSocketIoHttpResponse"/> 클래스를 사용해야 한다.
    /// </remarks>
    /// </summary>
    public class WebSocketIoHttpResponse : DefaultHttpResponse
    {
        private readonly HttpContext _context;

        public WebSocketIoHttpResponse(HttpContext context) : base(context)
        {
            _context = context;
            Cookies = new ResponseCookies(this.Headers, new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy()));
        }

        public override void Initialize(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
            }
            else
            {
                base.Initialize(context);
            }
        }

        public override void Uninitialize()
        {
            if (_context.WebSockets.IsWebSocketRequest)
            {
            }
            else
            {
                base.Uninitialize();
            }
        }

        public override void OnStarting(Func<object, Task> callback, object state)
        {
            if (_context.WebSockets.IsWebSocketRequest)
            {
            }
            else
            {
                base.OnStarting(callback, state);
            }
        }

        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            if (_context.WebSockets.IsWebSocketRequest)
            {
            }
            else
            {
                base.OnCompleted(callback, state);
            }
        }

        public override void Redirect(string location, bool permanent)
        {
            if (_context.WebSockets.IsWebSocketRequest)
            {
            }
            else
            {
                base.Redirect(location, permanent);
            }
        }

        public override HttpContext HttpContext => _context;

        public override int StatusCode { get; set; }

        public override IHeaderDictionary Headers { get; } = new HeaderDictionary { IsReadOnly = false };

        public override Stream Body { get; set; } = new MemoryStream();

        public override long? ContentLength { get; set; }

        public override string ContentType { get; set; }
        
        public override IResponseCookies Cookies { get; }

        public override bool HasStarted => false;

        public override void Redirect(string location)
        {
            if (_context.WebSockets.IsWebSocketRequest)
            {
            }
            else
            {
                base.Redirect(location);
            }
        }

        public override void OnStarting(Func<Task> callback)
        {
        }

        public override void OnCompleted(Func<Task> callback)
        {
        }
    }
}