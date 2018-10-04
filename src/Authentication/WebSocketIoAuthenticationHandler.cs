using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ntreev.AspNetCore.WebSocketIo.Authentication
{
    /// <summary>
    /// 웹소켓으로 JWT 인증이 필요한 경우 처리하는 핸들러입니다.
    /// </summary>
    public class WebSocketIoAuthenticationHandler : AuthenticationHandler<WebSocketIoOptions>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<WebSocketIoJwtOption> _jwtOption;

        public WebSocketIoAuthenticationHandler(IOptionsMonitor<WebSocketIoOptions> options, 
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IHttpContextAccessor httpContextAccessor,
            IOptions<WebSocketIoJwtOption> jwtOption) : base(options, logger, encoder, clock)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtOption = jwtOption;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!_httpContextAccessor.HttpContext.WebSockets.IsWebSocketRequest)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var packet = _httpContextAccessor.HttpContext.Items["web-socket-io-packet"] as WebSocketIoPacket;
            if (packet == null) return Task.FromResult(AuthenticateResult.NoResult());

            if (!packet.Headers.ContainsKey("authorization")) return Task.FromResult(AuthenticateResult.NoResult());

            if (_jwtOption != null)
            {
                var authorization = packet.Headers["authorization"].Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(authorization)) return Task.FromResult(AuthenticateResult.NoResult());
                if (!handler.CanValidateToken) return Task.FromResult(AuthenticateResult.NoResult());
                
                try
                {
                    var principal = handler.ValidateToken(authorization,
                        _jwtOption.Value.TokenValidationParameters, out var validatedToken);

                    return Task.FromResult(AuthenticateResult.Success(
                        new AuthenticationTicket(principal, WebSocketIoDefaults.AuthenticationScheme)));
                }
                catch(Exception e)
                {
                    return Task.FromResult(AuthenticateResult.Fail(e));
                }
            }

            var result = AuthenticateResult.Fail("Failed WebSocketIo authentication.");
            return Task.FromResult(result);
        }

        /// <summary>
        /// Response 동작이 없는 <see cref="HandleForbiddenAsync"/> 메서드로 재정의 합니다.
        /// </summary>
        /// <param name="properties">인증 속성</param>
        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Response 동작이 없는 <see cref="HandleChallengeAsync"/> 메서드로 재정의 합니다.
        /// </summary>
        /// <param name="properties">인증 속성</param>
        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }
    }
}