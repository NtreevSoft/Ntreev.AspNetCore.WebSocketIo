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
    /// <see cref="IServiceCollection"/> �������̽��� Ȯ�� �޼��� Ŭ���� �Դϴ�.
    /// </summary>
    public static class IServiceCollectionExtension
    {
        /// <summary>
        /// ASP.NET Core Mvc ��ɰ� �Բ� WebSocketIo ����� �߰��մϴ�.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static IMvcBuilder AddMvcWithWebSocketIo(this IServiceCollection services)
        {
            return AddMvcWithWebSocketIo(services, o => { });
        }

        /// <summary>
        /// ASP.NET Core Mvc ��ɰ� �Բ� WebSocketIo ����� �߰��մϴ�.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="setupAction">�ɼ� ����</param>
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