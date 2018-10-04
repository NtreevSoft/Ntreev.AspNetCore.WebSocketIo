using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;

namespace Ntreev.AspNetCore.WebSocketIo.Http
{
    /// <summary>
    /// 웹소켓을 지원하기 위한 <see cref="HttpContext"/> 클래스 입니다.
    /// </summary>
    public class WebSocketIoHttpContext : HttpContext
    {
        private readonly HttpContext _context;
        private ClaimsPrincipal _user;

        public WebSocketIoHttpContext(HttpContext context)
        {
            _context = context;
            _user = new WebSocketIoClaimsPrincipal(context.User, false);
            Response = new WebSocketIoHttpResponse(_context);
        }

        public override void Abort()
        {
        }

        public override IFeatureCollection Features => _context.Features;
        public override HttpRequest Request => _context.Request;
        public override HttpResponse Response { get; }
        public override ConnectionInfo Connection => _context.Connection;
        public override WebSocketManager WebSockets => _context.WebSockets;
        public override AuthenticationManager Authentication => _context.Authentication;

        public override ClaimsPrincipal User
        {
            get => _user;
            set => _user = value;
        }

        public override IDictionary<object, object> Items
        {
            get => _context.Items;
            set => _context.Items = value;
        }

        public override IServiceProvider RequestServices
        {
            get => _context.RequestServices;
            set => _context.RequestServices = value;
        }

        public override CancellationToken RequestAborted
        {
            get => _context.RequestAborted;
            set => _context.RequestAborted = value;
        }

        public override string TraceIdentifier
        {
            get => _context.TraceIdentifier;
            set => _context.TraceIdentifier = value;
        }

        public override ISession Session
        {
            get => _context.Session;
            set => _context.Session = value;
        }
    }
}