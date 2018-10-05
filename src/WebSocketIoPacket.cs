using System;
using System.Collections.Generic;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// �������� ��û ��Ŷ Ŭ���� �Դϴ�.
    /// </summary>
    public class WebSocketIoPacket : IWebSocketIoPacket
    {
        public WebSocketIoPacket()
        {
        }

        public WebSocketIoPacket(string id)
        {
            Id = id;
        }

        /// <inheritdoc cref="Id"/>
        public string Id { get; set; }

        /// <inheritdoc cref="Path"/>
        public string Path { get; set; }

        /// <inheritdoc cref="Headers"/>
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>(StringComparer.InvariantCulture);

        /// <inheritdoc cref="Data"/>
        public object Data { get; set; }
    }
}