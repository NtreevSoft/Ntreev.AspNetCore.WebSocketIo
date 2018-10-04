using System;
using System.Collections.Generic;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// 웹소켓의 요청 패킷 클래스 입니다.
    /// </summary>
    public class WebSocketIoPacket
    {
        /// <summary>
        /// 요청 고유 Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 노출된 API 의 경로
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// HTTP Header 에 대응되는 Header 입니다.
        /// </summary>
        public IDictionary<string, string> Headers = new Dictionary<string, string>(StringComparer.InvariantCulture);

        /// <summary>
        /// 요청 데이터 입니다.
        /// </summary>
        public object Data { get; set; }
    }
}