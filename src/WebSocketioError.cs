namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketIoError
    {
        public WebSocketIoErrorDetail Error { get; set; }
    }

    public class WebSocketIoErrorDetail
    {
        public WebSocketIoErrorDetail(string message, string detail)
        {
            Message = message;
            Detail = detail;
        }

        public string Message { get; }
        public string Detail { get; }
    }
}