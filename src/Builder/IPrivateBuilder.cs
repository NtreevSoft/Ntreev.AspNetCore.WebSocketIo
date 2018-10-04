using System;

namespace Ntreev.AspNetCore.WebSocketIo.Builder
{
    /// <summary>
    /// 비밀 메시지를 전달하는 빌더입니다.
    /// </summary>
    public interface IPrivateBuilder
    {
        /// <summary>
        /// 상대방의 <paramref name="socketId"/> 로 비밀 메시지를 보냅니다.
        /// </summary>
        /// <param name="socketId">웹소켓 Id</param>
        IWebSocketIo To(Guid socketId);
    }
}
