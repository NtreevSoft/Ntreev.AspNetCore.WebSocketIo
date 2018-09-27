using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ntreev.AspNetCore.WebSocketIo.Jwt
{
    public class JwtWebSocketIoBearerHandler : JwtBearerHandler
    {
        public JwtWebSocketIoBearerHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, IDataProtectionProvider dataProtection, ISystemClock clock) : base(options, logger, encoder, dataProtection, clock)
        {
        }
    }
}