using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ntreev.AspNetCore.WebSocketIo.Binder
{
    /// <summary>
    /// <see cref="WebSocketController"/> 에서 노출되는 API 의 매개변수를 바인딩 하는 바인더 입니다.
    /// </summary>
    public class WebSocketIoModelBinder : IModelBinder
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IHttpRequestStreamReaderFactory _requestStreamReaderFactory;
        private readonly MvcOptions _options;

        public WebSocketIoModelBinder(ILoggerFactory loggerFactory,
            IHttpRequestStreamReaderFactory requestStreamReaderFactory,
            MvcOptions options)
        {
            _loggerFactory = loggerFactory;
            _requestStreamReaderFactory = requestStreamReaderFactory;
            _options = options;
        }

        /// <inheritdoc cref="BindModelAsync"/>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.HttpContext.WebSockets.IsWebSocketRequest)
            {
                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (valueProviderResult != ValueProviderResult.None)
                {
                    bindingContext.Result = ModelBindingResult.Success(valueProviderResult.FirstValue);
                    return Task.CompletedTask;
                }

                // 바인딩 소스가 HTTP+Body 인 경우
                if (bindingContext.BindingSource == BindingSource.Body)
                {
                    var binder = new BodyModelBinder(_options.InputFormatters, 
                        _requestStreamReaderFactory,
                        _loggerFactory,
                        _options);

                    return binder.BindModelAsync(bindingContext);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(bindingContext.ModelName))
                {
                    if (!(bindingContext.HttpContext.Items["web-socket-io-packet"] is WebSocketIoPacket packet))
                        return Task.CompletedTask;

                    try
                    {
                        var obj = JsonConvert.DeserializeObject(packet.Data.ToString(), bindingContext.ModelType);
                        bindingContext.Result = ModelBindingResult.Success(obj);
                    }
                    catch
                    {
                        return Task.CompletedTask;
                    }
                }
                else
                {
                    var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                    if (valueProviderResult == ValueProviderResult.None) return Task.CompletedTask;

                    bindingContext.Result = ModelBindingResult.Success(valueProviderResult.FirstValue);
                }
            }

            return Task.CompletedTask;
        }
    }
}