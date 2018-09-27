using Microsoft.AspNetCore.Builder;
using Ntreev.AspNetCore.WebSocketIo.Middlewares;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    public static class WebSocketIoMiddlewareExtension
    {
        public static IApplicationBuilder UseWebSocketIo(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<WebSocketIoMiddleware>();

            return builder;
        }
    }
}