using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Ntreev.AspNetCore.WebSocketIo.Authentication;
using Ntreev.AspNetCore.WebSocketIo.Http;
using Ntreev.AspNetCore.WebSocketIo.Jwt;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    public static class JwtBearerExtensions
    {
        public static AuthenticationBuilder AddJwtBearerWithWebSocketIo(this AuthenticationBuilder builder)
        {
            return builder.AddJwtBearerWithWebSocketIo("Bearer", (Action<JwtBearerOptions>)(_ => { }));
        }

        public static AuthenticationBuilder AddJwtBearerWithWebSocketIo(this AuthenticationBuilder builder, Action<JwtBearerOptions> configureOptions)
        {
            return builder.AddJwtBearerWithWebSocketIo("Bearer", configureOptions);
        }

        public static AuthenticationBuilder AddJwtBearerWithWebSocketIo(this AuthenticationBuilder builder, string authenticationScheme, Action<JwtBearerOptions> configureOptions)
        {
            return builder.AddJwtBearerWithWebSocketIo(authenticationScheme, (string)null, configureOptions);
        }

        public static AuthenticationBuilder AddJwtBearerWithWebSocketIo(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<JwtBearerOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
            builder.AddScheme<JwtBearerOptions, JwtBearerHandler>(authenticationScheme, displayName, configureOptions);

            var jwtBearerOptions = new JwtBearerOptions();
            configureOptions(jwtBearerOptions);

            builder.Services.Configure<WebSocketIoJwtOption>(options => options.TokenValidationParameters = jwtBearerOptions.TokenValidationParameters);
            builder.AddScheme<WebSocketIoOptions, WebSocketIoAuthenticationHandler>(WebSocketIoDefaults.AuthenticationScheme, (options => { }));

            return builder;
        }
    }
}
