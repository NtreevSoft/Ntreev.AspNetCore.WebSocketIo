using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ntreev.AspNetCore.WebSocketIo.Authentication
{
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
                        new AuthenticationTicket(principal, WebSocketIoDefaults.AuthenticationSchema)));
                }
                catch(Exception e)
                {
                    return Task.FromResult(AuthenticateResult.Fail(e));
                }
            }

            var result = AuthenticateResult.Fail("Failed WebSocketIo authentication.");
            return Task.FromResult(result);
        }
    }
}