using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    /// <summary>
    /// ������ ��ü�� Ȯ�� �޼��� Ŭ���� �Դϴ�.
    /// </summary>
    public static class WebSocketExtension
    {
        /// <summary>
        /// �������� �����͸� �н��ϴ�.
        /// </summary>
        /// <param name="socket"><see cref="WebSocket"/> ��ü �Դϴ�.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> �Դϴ�.</param>
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
        /// Ŭ���̾�Ʈ�� �����͸� �����ϴ�.
        /// </summary>
        /// <param name="socket"><see cref="WebSocket"/> ��ü �Դϴ�.</param>
        /// <param name="data">���ڿ� ������ �Դϴ�.</param>
        /// <param name="endOfMessage">���� �޽����� �������� ������ �ƴ��� �����Դϴ�.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> �Դϴ�.</param>
        public static async Task SendDataAsync(this WebSocket socket, string data, bool endOfMessage = true,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));

            await socket.SendAsync(buffer, WebSocketMessageType.Text, endOfMessage, cancellationToken);
        }
    }
}