using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Ntreev.AspNetCore.WebSocketIo.Filters
{
    /// <summary>
    /// HTTP 연결인 경우 웹소켓을 생성하고 소멸할 때의 라이프사이클을 추적하기 위한 필터 입니다.
    /// </summary>
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

        /// <inheritdoc cref="IActionFilter.OnActionExecuted"/>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.WebSockets.IsWebSocketRequest) return;

            // 라이프사이클이 끝나면 웹소켓 제거
            var webSocketIo = _serviceProvider.GetService<IWebSocketIo>();
            _webSocketIoConnectionManager.RemoveAsync(webSocketIo.SocketId).GetAwaiter();
        }

        /// <inheritdoc cref="IAsyncActionFilter.OnActionExecutionAsync"/>
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
                // 라이프사이클이 끝나면 소속된 모든 채널(방)과 소켓 목록에서 제거한다.
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
