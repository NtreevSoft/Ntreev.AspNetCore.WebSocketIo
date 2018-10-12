using System.Collections.Generic;

namespace Ntreev.AspNetCore.WebSocketIo.Builder
{
    /// <summary>
    /// 브로드캐스트 메시지를 전달하는 빌더입니다.
    /// </summary>
    public interface IBroadcastBuilder
    {
        /// <summary>
        /// 나를 제외한 모두의 웹소켓 목록을 반환합니다.
        /// </summary>
        IEnumerable<IWebSocketIo> To();

        /// <summary>
        /// 나를 제외한 모두의 웹소켓 목록을 반환합니다.
        /// </summary>
        /// <param name="channelKey">채널(방) 이름</param>
        IEnumerable<IWebSocketIo> To(string channelKey);

        /// <summary>
        /// 나를 포함한 모두의 웹소켓 목록을 반환합니다.
        /// </summary>
        IEnumerable<IWebSocketIo> In();

        /// <summary>
        /// 나를 포함한 모두의 웹소켓 목록을 반환합니다.
        /// </summary>
        /// <param name="channelKey">채널(방) 이름</param>
        IEnumerable<IWebSocketIo> In(string channelKey);
    }
}
