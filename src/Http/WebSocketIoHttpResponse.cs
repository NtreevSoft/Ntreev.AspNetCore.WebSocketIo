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
    /// �������� �����ϱ� ���� <see cref="HttpResponse"/> Ŭ���� �Դϴ�.
    /// <remarks>
    /// �������� Ư�� �� �ѹ��� HTTP ���׷��̵带 ���� HTTP �� ��û�ϰ�, ������ ������ �ȴ�.
    /// ASP.NET Core �� Response �� ���۵Ǹ� �ٽ� �� Response �� ������ �� ���� ���ܰ� �߻��ϰ� �ȴ�.
    /// ������ ������ ��� ���� �߻��� �����ϱ� ���� <see cref="WebSocketIoHttpResponse"/> Ŭ������ ����ؾ� �Ѵ�.
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