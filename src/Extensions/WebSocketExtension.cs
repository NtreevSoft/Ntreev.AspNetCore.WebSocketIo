using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    /// <summary>
    /// 웹소켓 객체의 확장 메서드 클래스 입니다.
    /// </summary>
    public static class WebSocketExtension
    {
        /// <summary>
        /// 웹소켓의 데이터를 읽습니다.
        /// </summary>
        /// <param name="socket"><see cref="WebSocket"/> 객체 입니다.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> 입니다.</param>
        public static async Task<string> ReadDataAsync(this WebSocket socket,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[4096]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result = null;
                do
                {
                    result = await socket.ReceiveAsync(buffer, cancellationToken);
                    await ms.WriteAsync(buffer.Array, buffer.Offset, result.Count, cancellationToken);
                } while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);

                using (var sr = new StreamReader(ms, Encoding.UTF8))
                {
                    return await sr.ReadToEndAsync();
                }
            }
        }

        /// <summary>
        /// 클라이언트로 데이터를 보냅니다.
        /// </summary>
        /// <param name="socket"><see cref="WebSocket"/> 객체 입니다.</param>
        /// <param name="data">문자열 데이터 입니다.</param>
        /// <param name="endOfMessage">현재 메시지가 데이터의 끝인지 아닌지 여부입니다.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> 입니다.</param>
        public static async Task SendDataAsync(this WebSocket socket, string data, bool endOfMessage = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));

            await socket.SendAsync(buffer, WebSocketMessageType.Text, endOfMessage, cancellationToken);
        }
    }
}