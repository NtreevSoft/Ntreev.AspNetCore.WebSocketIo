using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Ntreev.AspNetCore.WebSocketIo.Extensions;
using Ntreev.AspNetCore.WebSocketIo.Http;

namespace Ntreev.AspNetCore.WebSocketIo.Middlewares
{
    /// <summary>
    /// 웹소켓 연결에 대한 입출력과 라이프사이클을 담당하는 미들웨어 클래스 입니다.
    /// </summary>
    public class WebSocketIoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketIoConnectionManager _webSocketIoConnectionManager;

        public WebSocketIoMiddleware(RequestDelegate next,
            IWebSocketIoConnectionManager webSocketIoConnectionManager)
        {
            _next = next;
            _webSocketIoConnectionManager = webSocketIoConnectionManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                // HTTP 연결인 경우 일회용으로 사용할 빈 웹소켓 객체를 생성한다.
                var httpWebSocketIoId = Guid.NewGuid();
                var httpWebSocketIo = new WebSocketIo(httpWebSocketIoId, new EmptyWebSocket(), _webSocketIoConnectionManager);
                context.Items["web-socket-io"] = httpWebSocketIo;
                context.Items["web-socket-io-packet"] = new WebSocketIoPacket(Guid.NewGuid().ToString());

                _webSocketIoConnectionManager.Add(httpWebSocketIoId, httpWebSocketIo);
                _webSocketIoConnectionManager.OnConnected(this, httpWebSocketIo);

                await _next(context);
                return;
            }
            
            context = new WebSocketIoHttpContext(context);

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var webSocketIoId = Guid.NewGuid();
            var webSocketIo = new WebSocketIo(webSocketIoId, socket, _webSocketIoConnectionManager);
            context.Items["web-socket-io"] = webSocketIo;

            _webSocketIoConnectionManager.Add(webSocketIoId, webSocketIo);
            _webSocketIoConnectionManager.OnConnected(this, webSocketIo);

            while (socket.State == WebSocketState.Open)
            {
                var data = await socket.ReadDataAsync(CancellationToken.None);

                switch (socket.State)
                {
                    case WebSocketState.Connecting:
                        continue;

                    case WebSocketState.CloseReceived:
                    case WebSocketState.CloseSent:
                    case WebSocketState.Closed:
                        webSocketIo.OnDisconnecting(this);
                        await _webSocketIoConnectionManager.DisposeAsync(webSocketIo);
                        _webSocketIoConnectionManager.OnDisconnected(this, webSocketIo);
                        return;
                }

                WebSocketIoPacket packet = null;
                try
                {
                    packet = JsonConvert.DeserializeObject<WebSocketIoPacket>(data);
                    if (packet == null) continue;
                }
                catch (Exception e)
                {
                    var error = new WebSocketIoError
                    {
                        Id = packet.Id,
                        Error = new WebSocketIoErrorDetail(e.Message, e.ToString())
                    };
                    await webSocketIo.Socket.SendDataAsync(error.ToJson());
                    continue;
                }

                context.Items["web-socket-io-data"] = data;
                context.Items["web-socket-io-packet"] = packet;

                var url = packet.Path.Split('?');

                context.Request.Path = url[0];
                context.Request.QueryString = new QueryString(url.Length > 1 ? "?" + url[1] : string.Empty);

                try
                {
                    await _next(context);
                }
                catch (Exception e)
                {
                    var error = new WebSocketIoError
                    {
                        Id = packet.Id,
                        Error = new WebSocketIoErrorDetail(e.Message, e.ToString())
                    };
                    await webSocketIo.Socket.SendDataAsync(error.ToJson());
                }
            }

            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }
    }
}