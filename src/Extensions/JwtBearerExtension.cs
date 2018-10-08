using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Ntreev.AspNetCore.WebSocketIo.Authentication;

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
