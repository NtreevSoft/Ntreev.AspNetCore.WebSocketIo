using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Ntreev.AspNetCore.WebSocketIo.Authentication;
using Ntreev.AspNetCore.WebSocketIo.Binder;
using Ntreev.AspNetCore.WebSocketIo.Filters;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> 인터페이스의 확장 메서드 클래스 입니다.
    /// </summary>
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// ASP.NET Core Mvc 기능과 함께 WebSocketIo 기능을 추가합니다.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static IMvcBuilder AddMvcWithWebSocketIo(this IServiceCollection services)
        {
            return AddMvcWithWebSocketIo(services, o => { });
        }

        /// <summary>
        /// ASP.NET Core Mvc 기능과 함께 WebSocketIo 기능을 추가합니다.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="setupAction">옵션 설정</param>
        public static IMvcBuilder AddMvcWithWebSocketIo(this IServiceCollection services, Action<MvcOptions> setupAction)
        {
            setupAction = setupAction + (options =>
            {
                options.ModelBinderProviders.Insert(0, new WebSocketIoModelBinderProvider(options));
                options.Filters.Add(typeof(AuthorizeAttributeTrackingFilter), int.MinValue);
                options.Filters.Add(typeof(HttpWebSocketIoDisposableFilter), int.MaxValue);
            });

            var mvcBuilder = services.AddMvc(setupAction);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IWebSocketIoConnectionManager, WebSocketIoConnectionManager>()
                .AddScoped<IAuthenticationHandlerProvider, WebSocketIoAuthenticationHandlerProvider>()
                .AddTransient<IWebSocketIo>(provider =>
                {
                    var accessor = provider.GetService<IHttpContextAccessor>();
                    var webSocketIo = accessor.HttpContext.Items["web-socket-io"] as IWebSocketIo;
                    if (webSocketIo == null)
                        throw new NullReferenceException(nameof(webSocketIo));

                    return webSocketIo;
                });

            return mvcBuilder;
        }
    }
}