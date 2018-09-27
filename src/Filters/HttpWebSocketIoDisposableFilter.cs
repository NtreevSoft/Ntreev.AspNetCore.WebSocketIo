using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Ntreev.AspNetCore.WebSocketIo.Filters
{
    public class HttpWebSocketIoDisposableFilter : IActionFilter, IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebSocketIoConnectionManager _webSocketIoConnectionManager;

        public HttpWebSocketIoDisposableFilter(IServiceProvider serviceProvider,
            IWebSocketIoConnectionManager webSocketIoConnectionManager)
        {
            _serviceProvider = serviceProvider;
            _webSocketIoConnectionManager = webSocketIoConnectionManager;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.WebSockets.IsWebSocketRequest) return;

            var webSocketIo = _serviceProvider.GetService<IWebSocketIo>();
            _webSocketIoConnectionManager.RemoveAsync(webSocketIo.SocketId).GetAwaiter();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.WebSockets.IsWebSocketRequest)
            {
                await next();
                return;
            }

            try
            {
                await next();
            }
            finally
            {
                var webSocketIo = _serviceProvider.GetService<IWebSocketIo>();
                var rooms = new List<string>(webSocketIo.JoinedRooms);

                foreach (var room in rooms)
                {
                    await _webSocketIoConnectionManager.LeaveAsync(room, webSocketIo);
                }

                _webSocketIoConnectionManager.RemoveAsync(webSocketIo.SocketId).GetAwaiter();
            }
        }
    }
}
