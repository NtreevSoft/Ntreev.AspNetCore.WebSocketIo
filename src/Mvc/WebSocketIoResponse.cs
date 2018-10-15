using Newtonsoft.Json;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    /// <summary>
    /// 웹소켓의 응답 패킷 클래스 입니다.
    /// </summary>
    public class WebSocketIoResponse
    {
        public WebSocketIoResponse(string id, object data) : this(id, 200, data)
        {
        }

        public WebSocketIoResponse(string id, int? statusCode, object data) 
            : this(id, WebSocketIoResponseType.Message, statusCode, data)
        {
        }

        public WebSocketIoResponse(string id, WebSocketIoResponseType type, int? statusCode, object data)
            : this(id, type, null, statusCode, data)
        {
        }

        public WebSocketIoResponse(string id, WebSocketIoResponseType type, string emitName, int? statusCode, object data)
        {
            Id = id;
            Type = type;
            EmitName = emitName;
            StatusCode = statusCode;
            Data = data;
        }

        /// <summary>
        /// 웹소켓의 요청에서 사용한 <see cref="WebSocketIoPacket.Id"/> 값입니다.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 서버에서 이벤트가 발생하는 경우 이벤트 이름입니다.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmitName { get; }

        /// <summary>
        /// 서버의 메시지의 타입 입니다.
        /// </summary>
        public WebSocketIoResponseType Type { get; }

        /// <summary>
        /// 상태 코드입니다. 대체로 HTTP 의 상태 코드의 값이 사용될 수 있습니다.
        /// </summary>
        public int? StatusCode { get; }

        /// <summary>
        /// 서버에서 전송하는 데이터 입니다.
        /// </summary>
        public object Data { get; }

        public static WebSocketIoResponse CreateMessage(IWebSocketIoPacket packet, object data)
        {
            return new WebSocketIoResponse(packet.Id, data);
        }

        public static WebSocketIoResponse CreateEvent(IWebSocketIoPacket packet, string emitName, object data)
        {
            return new WebSocketIoResponse(packet.Id, WebSocketIoResponseType.Event, emitName, 200, data);
        }
    }
}
