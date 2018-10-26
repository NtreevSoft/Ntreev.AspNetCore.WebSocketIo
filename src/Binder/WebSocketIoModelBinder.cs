using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ntreev.AspNetCore.WebSocketIo.Binder
{
    /// <summary>
    /// <see cref="WebSocketIoController"/> ���� ����Ǵ� API �� �Ű������� ���ε� �ϴ� ���δ� �Դϴ�.
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
                    var converter = TypeDescriptor.GetConverter(bindingContext.ModelType);
                    var convertedObject = converter.ConvertFrom(valueProviderResult.FirstValue);

                    bindingContext.Result = ModelBindingResult.Success(convertedObject);
                    return Task.CompletedTask;
                }

                // ���ε� �ҽ��� HTTP+Body �� ���
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

                    var converter = TypeDescriptor.GetConverter(bindingContext.ModelType);
                    var convertedObject = converter.ConvertFrom(valueProviderResult.FirstValue);

                    bindingContext.Result = ModelBindingResult.Success(convertedObject);
                }
            }

            return Task.CompletedTask;
        }
    }
}