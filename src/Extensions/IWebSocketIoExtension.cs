using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ntreev.AspNetCore.WebSocketIo.Mvc;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    /// <summary>
    /// WebSocketIo 의 확장 메서드 클래스 입니다.
    /// </summary>
    public static class IWebSocketIoExtension
    {
        /// <summary>
        /// 클라이언트에 데이터를 전송합니다.
        /// </summary>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/></param>
        /// <param name="data">데이터 입니다.</param>
        /// <param name="endOfMessage">메시지의 끝인지 아닌지의 여부 입니다.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> 입니다.</param>
        public static Task SendDataAsync(this IWebSocketIo webSocketIo, string data, bool endOfMessage = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return webSocketIo.SendDataAsync(data, endOfMessage, cancellationToken);
        }

        /// <summary>
        /// 클라이언트에게 객체를 전송합니다.
        /// </summary>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/></param>
        /// <param name="obj">객체 입니다.</param>
        /// <param name="endOfMessage">메시지의 끝인지 아닌지의 여부 입니다.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> 입니다.</param>
        public static Task SendDataAsync(this IWebSocketIo webSocketIo, object obj, bool endOfMessage = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return webSocketIo.SendDataAsync(obj, endOfMessage, cancellationToken);
        }

        /// <summary>
        /// 클라이언트에게 데이터를 전송합니다.
        /// </summary>
        /// <param name="webSocketIos"><see cref="IWebSocketIo"/> 목록입니다.</param>
        /// <param name="data">데이터 입니다.</param>
        /// <param name="endOfMessage">메시지의 끝인지 아닌지의 여부 입니다.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> 입니다.</param>
        public static async Task SendDataAsync(this IEnumerable<IWebSocketIo> webSocketIos, string data,
            bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var webSocketIo in webSocketIos)
            {
                await SendDataAsync(webSocketIo, data, endOfMessage, cancellationToken);
            }
        }

        /// <summary>
        /// 클라이언트에게 데이터를 전송합니다.
        /// </summary>
        /// <param name="webSocketIos"><see cref="IWebSocketIo"/> 목록입니다.</param>
        /// <param name="obj">데이터 객체 입니다.</param>
        /// <param name="endOfMessage">메시지의 끝인지 아닌지의 여부 입니다.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> 입니다.</param>
        /// <returns></returns>
        public static async Task SendDataAsync(this IEnumerable<IWebSocketIo> webSocketIos, object obj,
            bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var webSocketIo in webSocketIos)
            {
                await SendDataAsync(webSocketIo, obj, endOfMessage, cancellationToken);
            }
        }

        /// <summary>
        /// 클라이언트에게 데이터를 전송합니다.
        /// </summary>
        /// <param name="webSocketIos"><see cref="IWebSocketIo"/> 목록입니다.</param>
        /// <param name="response">응답 패킷 입니다.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> 입니다.</param>
        /// <returns></returns>
        public static async Task SendDataAsync(this IEnumerable<IWebSocketIo> webSocketIos,
            WebSocketIoResponse response, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var webSocketIo in webSocketIos)
            {
                await SendDataAsync(webSocketIo, response, true, cancellationToken);
            }
        }

        /// <summary>
        /// 웹소켓이 서버에 연결할 때 HTTP 사양을 업그레이드 하고, 클라이언트와 연결되는데 이것은 HTTP 요청입니다.
        /// 웹소켓 요청을 위한 연결인지와 웹소켓 연결인지의 여부를 반환하는 메서드 입니다.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/></param>
        public static bool IsWebSocketRequestOrConnection(this HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("upgrade"))
            {
                var value = context.Request.Headers["upgrade"];
                if (value.ToString().ToLower() == "websocket")
                {
                    return true;
                }
            }

            return context.WebSockets.IsWebSocketRequest;
        }
    }
}
