namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// 웹소켓이 연결/연결해제 될 때 발생하는 이벤트 클래스 입니다.
    /// </summary>
    public class WebSocketIoEventArgs
    {
        public WebSocketIoEventArgs(string roomKey, IWebSocketIo webSocketIo)
        {
            RoomKey = roomKey;
            WebSocketIo = webSocketIo;
        }

        /// <summary>
        /// 채널(방) 키 입니다.
        /// </summary>
        public string RoomKey { get; }

        /// <summary>
        /// <see cref="IWebSocketIo"/> 객체 입니다.
        /// </summary>
        public IWebSocketIo WebSocketIo { get; }
    }
}
