using System;
using System.Collections.Generic;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// �������� ��û ��Ŷ Ŭ���� �Դϴ�.
    /// </summary>
    public class WebSocketIoPacket
    {
        /// <summary>
        /// ��û ���� Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ����� API �� ���
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// HTTP Header �� �����Ǵ� Header �Դϴ�.
        /// </summary>
        public IDictionary<string, string> Headers = new Dictionary<string, string>(StringComparer.InvariantCulture);

        /// <summary>
        /// ��û ������ �Դϴ�.
        /// </summary>
        public object Data { get; set; }
    }
}