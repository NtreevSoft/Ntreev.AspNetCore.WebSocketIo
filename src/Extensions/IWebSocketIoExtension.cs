using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    public static class IWebSocketIoExtension
    {
        public static Task SendDataAsync(this IWebSocketIo webSocketIo, string data, bool endOfMessage = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return webSocketIo.SendDataAsync(data, endOfMessage, cancellationToken);
        }

        public static Task SendDataAsync(this IWebSocketIo webSocketIo, object obj, bool endOfMessage = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return webSocketIo.SendDataAsync(obj, endOfMessage, cancellationToken);
        }

        public static async Task SendDataAsync(this IEnumerable<IWebSocketIo> webSocketIos, string data,
            bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var webSocketIo in webSocketIos)
            {
                await SendDataAsync(webSocketIo, data, endOfMessage, cancellationToken);
            }
        }

        public static async Task SendDataAsync(this IEnumerable<IWebSocketIo> webSocketIos, object obj,
            bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var webSocketIo in webSocketIos)
            {
                await SendDataAsync(webSocketIo, obj, endOfMessage, cancellationToken);
            }
        }
    }
}
