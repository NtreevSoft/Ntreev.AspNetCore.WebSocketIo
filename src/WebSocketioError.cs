namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// 웹소켓의 예외 정보를 담는 클래스 입니다.
    /// </summary>
    public class WebSocketIoError
    {
        public string Id { get; set; }
        public WebSocketIoErrorDetail Error { get; set; }
    }

    /// <summary>
    /// 웹소켓의 예외의 상세한 정보를 담는 클래스 입니다.
    /// </summary>
    public class WebSocketIoErrorDetail
    {
        public WebSocketIoErrorDetail(string message, string detail)
        {
            Message = message;
            Detail = detail;
        }

        /// <summary>
        /// 예외 메시지 입니다.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 예외 상세 메시지 입니다.
        /// </summary>
        public string Detail { get; }
    }
}