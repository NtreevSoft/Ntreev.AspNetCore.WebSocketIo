using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ntreev.AspNetCore.WebSocketIo.Binder
{
    /// <summary>
    /// <see cref="WebSocketIoController"/> �� ����Ǵ� API �� ȣ���ϱ� ���� ���δ��� �����մϴ�.
    /// </summary>
    public class WebSocketIoModelBinderProvider : IModelBinderProvider
    {
        private readonly MvcOptions _options;

        public WebSocketIoModelBinderProvider(MvcOptions options)
        {
            _options = options;
        }

        /// <inheritdoc cref="GetBinder"/>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var loggerFactory = context.Services.GetService<ILoggerFactory>();
            var requestStreamReaderFactory = context.Services.GetService<IHttpRequestStreamReaderFactory>();
            
            return new WebSocketIoModelBinder(loggerFactory, requestStreamReaderFactory, _options);
        }
    }
}