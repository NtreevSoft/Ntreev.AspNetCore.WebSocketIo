using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo;
using Ntreev.AspNetCore.WebSocketIo.Extensions;

namespace WebSocketIo_Web.Controllers.Api
{
    [Route("/api/rooms/{roomName}")]
    public class RoomsController : WebSocketController
    {
        private readonly IWebSocketIo _webSocketIo;

        public RoomsController(IWebSocketIo webSocketIo) : base(webSocketIo)
        {
            _webSocketIo = webSocketIo;
        }

        [Route("join")]
        public async Task<IActionResult> JoinAsync(string roomName)
        {
            _webSocketIo.Disconnecting += WebSocketIoOnDisconnecting;
            _webSocketIo.Leaved += WebSocketIoOnLeaved;

            await _webSocketIo.JoinAsync(roomName);
            await _webSocketIo.Broadcast.To(roomName).SendDataAsync($"Join {_webSocketIo.SocketId} in {roomName} room.");

            return Ok();
        }

        [Route("leave")]
        public async Task<IActionResult> LeaveAsync(string roomName)
        {
            await _webSocketIo.LeaveAsync(roomName);
            await _webSocketIo.Broadcast.In(roomName).SendDataAsync($"Leave {_webSocketIo.SocketId} in {roomName} room.");

            return Ok();
        }

        [Route("chat/{message}")]
        public async Task<IActionResult> ChatAsync(string roomName, string message)
        {
            await _webSocketIo.Broadcast.In(roomName).SendDataAsync($"Chat: {message}");

            return Ok();
        }

        [Route("whisper/{socketId}/{message}")]
        public async Task<IActionResult> WhisperAsync(string roomName, string socketId, string message)
        {
            await _webSocketIo.Private.To(Guid.Parse(socketId)).SendDataAsync(message);

            return Ok();
        }

        private async void WebSocketIoOnLeaved(object sender, WebSocketIoEventArgs e)
        {
            await e.WebSocketIo.Broadcast.To(e.RoomKey).SendDataAsync($"Leaved {e.WebSocketIo.SocketId} in {e.RoomKey}");
        }

        private async void WebSocketIoOnDisconnecting(object sender, EventArgs e)
        {
            foreach (var room in _webSocketIo.JoinedRooms)
            {
                await _webSocketIo.Broadcast.In(room).SendDataAsync($"Disconnect {_webSocketIo.SocketId} in {room} room");
            }
        }
    }
}
