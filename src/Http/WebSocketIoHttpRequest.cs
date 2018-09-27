using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Ntreev.AspNetCore.WebSocketIo.Http
{
    public class WebSocketIoHttpRequest : DefaultHttpRequest
    {
        private readonly HttpContext _context;

        public WebSocketIoHttpRequest(HttpContext context) : base(context)
        {
            _context = context;
        }
    }
}
