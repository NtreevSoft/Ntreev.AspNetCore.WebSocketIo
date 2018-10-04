using Microsoft.AspNetCore.Builder;
using Ntreev.AspNetCore.WebSocketIo.Middlewares;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    /// <summary>
    /// WebSocketIo �̵���� Ȯ�� �޼��� Ŭ���� �Դϴ�.
    /// </summary>
    public static class WebSocketIoMiddlewareExtension
    {
        /// <summary>
        /// WebSocketIo �̵��� ����մϴ�.
        /// </summary>
        /// <param name="builder"><see cref="IApplicationBuilder"/> ��ü �Դϴ�.</param>
        public static IApplicationBuilder UseWebSocketIo(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<WebSocketIoMiddleware>();

            return builder;
        }
    }
}