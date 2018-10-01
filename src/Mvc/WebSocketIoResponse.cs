using System;
using System.Collections.Generic;
using System.Text;

namespace Ntreev.AspNetCore.WebSocketIo.Mvc
{
    public class WebSocketIoResponse
    {
        public WebSocketIoResponse(string id, object data) : this(id, 200, data)
        {
        }

        public WebSocketIoResponse(string id, int statusCode, object data) : this(id, WebSocketIoResponseType.Message, statusCode, data)
        {
        }

        public WebSocketIoResponse(string id, WebSocketIoResponseType type, int statusCode, object data)
        {
            Id = id;
            Type = type;
            StatusCode = statusCode;
            Data = data;
        }

        public string Id { get; }
        public WebSocketIoResponseType Type { get; }
        public int StatusCode { get; }
        public object Data { get; }
    }
}
