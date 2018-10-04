using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace Ntreev.AspNetCore.WebSocketIo.Authentication
{
    /// <summary>
    /// HTTP 또는 WebSocket 별로 인증 핸들러를 제공하는 프로바이더 입니다.
    /// </summary>
    public class WebSocketIoAuthenticationHandlerProvider : IAuthenticationHandlerProvider
    {
        private readonly IAuthenticationSchemeProvider _schemes;

        public WebSocketIoAuthenticationHandlerProvider(IAuthenticationSchemeProvider schemes)
        {
            _schemes = schemes;
        }

        /// <summary>
        /// 웹소켓 요청도 Bearer 스키마로 인증 요청이 오기 때문에 웹소켓인 경우 WebSocketIo 스키마로 변경해서 인증 핸들러를 반환하도록 합니다.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/></param>
        /// <param name="authenticationScheme">인증 스키마</param>
        public async Task<IAuthenticationHandler> GetHandlerAsync(HttpContext context, string authenticationScheme)
        {
            var scheme = await _schemes.GetSchemeAsync(authenticationScheme);
            if (scheme == null) return null;

            // 웹소켓 요청인 경우에도 Bearer 스키마를 반환하기 때문에 WebSocketIo 스키마로 변경한다.
            if (context.IsWebSocketRequestOrConnection())
            {
                scheme = await _schemes.GetSchemeAsync(WebSocketIoDefaults.AuthenticationScheme);
            }

            var handler = ActivatorUtilities.CreateInstance(context.RequestServices, scheme.HandlerType) as IAuthenticationHandler;
            if (handler != null)
            {
                await handler.InitializeAsync(scheme, context);
            }

            return handler;
        }
    }
}
