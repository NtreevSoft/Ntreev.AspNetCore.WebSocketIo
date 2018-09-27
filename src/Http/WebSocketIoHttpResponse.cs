using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.ObjectPool;

namespace Ntreev.AspNetCore.WebSocketIo.Http
{
    public class WebSocketIoHttpResponse : DefaultHttpResponse
    {
        private readonly HttpContext _context;

        public WebSocketIoHttpResponse(HttpContext context) : base(context)
        {
            _context = context;
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

        public override int StatusCode
        {
            get => 200;
            set { }
        }

        public override IHeaderDictionary Headers => new HeaderDictionary();

        public override Stream Body
        {
            get => new MemoryStream();
            set { }
        }

        public override long? ContentLength
        {
            get => 0;
            set { }
        }

        public override string ContentType
        {
            get => string.Empty;
            set { }
        }
        
        public override IResponseCookies Cookies => new ResponseCookies(this.Headers, new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy()));

        public override bool HasStarted => true;
        
        public override void OnStarting(Func<Task> callback)
        {
            base.OnStarting(callback);
        }

        public override void RegisterForDispose(IDisposable disposable)
        {
            base.RegisterForDispose(disposable);
        }

        public override void OnCompleted(Func<Task> callback)
        {
            base.OnCompleted(callback);
        }

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
    }
}