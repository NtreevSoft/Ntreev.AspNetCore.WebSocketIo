namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketIoEventArgs
    {
        public WebSocketIoEventArgs(string roomKey, IWebSocketIo webSocketIo)
        {
            RoomKey = roomKey;
            WebSocketIo = webSocketIo;
        }

        public string RoomKey { get; }
        public IWebSocketIo WebSocketIo { get; }
    }
}
