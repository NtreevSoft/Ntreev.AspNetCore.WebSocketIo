using Microsoft.AspNetCore.Builder;
using Ntreev.AspNetCore.WebSocketIo.Middlewares;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    /// <summary>
    /// WebSocketIo 미들웨어 확장 메서드 클래스 입니다.
    /// </summary>
    public static class WebSocketIoMiddlewareExtension
    {
        /// <summary>
        /// WebSocketIo 미들웨어를 사용합니다.
        /// </summary>
        /// <param name="builder"><see cref="IApplicationBuilder"/> 객체 입니다.</param>
        public static IApplicationBuilder UseWebSocketIo(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<WebSocketIoMiddleware>();

            return builder;
        }
    }
}