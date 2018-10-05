using System.Collections.Generic;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// 웹소켓 패킷을 정의하는 인터페이스 입니다.
    /// </summary>
    public interface IWebSocketIoPacket
    {
        /// <summary>
        /// 요청 고유 Id 입니다.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// 노출된 API 경로 입니다.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// 데이터 입니다.
        /// </summary>
        object Data { get; set; }

        /// <summary>
        /// 헤더 데이터 입니다.
        /// </summary>
        IDictionary<string, string> Headers { get; }
    }
}