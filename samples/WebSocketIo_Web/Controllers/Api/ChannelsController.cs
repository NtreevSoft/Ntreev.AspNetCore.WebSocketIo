using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ntreev.AspNetCore.WebSocketIo;
using Ntreev.AspNetCore.WebSocketIo.Extensions;
using Ntreev.AspNetCore.WebSocketIo.Mvc;

namespace WebSocketIo_Web.Controllers.Api
{
    [Route("/api/channels/{channelName}")]
    public class ChannelsController : WebSocketController
    {
        private readonly IWebSocketIo _webSocketIo;
        private readonly IWebSocketIoPacket _packet;

        public ChannelsController(IWebSocketIo webSocketIo,
            IWebSocketIoPacket packet) : base(webSocketIo)
        {
            _webSocketIo = webSocketIo;
            _packet = packet;
        }

        [Route("join")]
        public async Task<IActionResult> JoinAsync(string channelName)
        {
            _webSocketIo.Disconnecting += WebSocketIoOnDisconnecting;
            _webSocketIo.Leaved += WebSocketIoOnLeaved;

            await _webSocketIo.JoinAsync(channelName);
            await _webSocketIo.Broadcast.In(channelName).SendDataAsync(
                WebSocketIoResponse.CreateEvent(_packet, "chat.joined", $"Joined {_webSocketIo.SocketId} in {channelName}."));

            return Ok();
        }

        [Route("leave")]
        public async Task<IActionResult> LeaveAsync(string channelName)
        {
            await _webSocketIo.LeaveAsync(channelName);
            await _webSocketIo.Broadcast.In(channelName).SendDataAsync($"Leave {_webSocketIo.SocketId} in {channelName} room.");

            return Ok();
        }

        [Route("chat")]
        public async Task<IActionResult> ChatAsync(string channelName, ChatRequest request)
        {
            await _webSocketIo.Broadcast.In(channelName).SendDataAsync(
                WebSocketIoResponse.CreateEvent(_packet, "chat.received", request.Message));

            return Ok();
        }

        [Route("whisper/{socketId}/{message}")]
        public async Task<IActionResult> WhisperAsync(string channelName, string socketId, string message)
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

    public class ChatRequest
    {
        public string Message { get; set; }
    }
}
