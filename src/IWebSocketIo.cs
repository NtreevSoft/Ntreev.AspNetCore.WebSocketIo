using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Ntreev.AspNetCore.WebSocketIo.Builder;
using Ntreev.AspNetCore.WebSocketIo.Mvc;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// 웹소켓 기능을 제공하는 인터페이스 입니다.
    /// </summary>
    public interface IWebSocketIo
    {
        /// <summary>
        /// 웹소켓의 Id
        /// </summary>
        Guid SocketId { get; }

        /// <summary>
        /// <see cref="WebSocket"/> 객체
        /// </summary>
        WebSocket Socket { get; }

        /// <summary>
        /// 브로드캐스트 메시지 빌더 입니다.
        /// </summary>
        IBroadcastBuilder Broadcast { get; }

        /// <summary>
        /// 비밀 메시지를 보내기 위한 빌더 입니다.
        /// </summary>
        IPrivateBuilder Private { get; }

        /// <summary>
        /// 웹소켓이 소속된 채널(방) 목록 입니다.
        /// </summary>
        IList<string> JoinedChannels { get; }

        /// <summary>
        /// 웹소켓 연결이 끊기거나 사용자가 떠나면 발생하는 이벤트 입니다.
        /// </summary>
        event EventHandler<WebSocketIoEventArgs> Leaved;

        /// <summary>
        /// 웹소켓 연결이 끊기거나 사용자가 떠나기 전에 발생하는 이벤트 입니다.
        /// </summary>
        event EventHandler Disconnecting;

        /// <summary>
        /// <see cref="Leaved"/> 이벤트를 발생합니다.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">매개변수</param>
        void OnLeaved(object sender, WebSocketIoEventArgs args);

        /// <summary>
        /// <see cref="Disconnecting"/> 이벤트를 발생합니다.
        /// </summary>
        /// <param name="sender">Sender</param>
        void OnDisconnecting(object sender);

        /// <summary>
        /// 클라이언트에게 데이터를 전송합니다.
        /// </summary>
        /// <param name="data">데이터</param>
        /// <param name="endOfMessage">메시지의 끝인지 아닌지 여부입니다.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> 입니다.</param>
        Task SendDataAsync(string data, bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 클라이언트에게 데이터 객체를 전송합니다.
        /// </summary>
        /// <param name="obj">데이터 객체</param>
        /// <param name="endOfMessage">메시지의 끝인지 아닌지 여부입니다.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> 입니다.</param>
        /// <returns></returns>
        Task SendDataAsync(object obj, bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 클라이언트에게 웹소켓 응답 패킷을 전송합니다.
        /// </summary>
        /// <param name="response">웹소켓 응답 패킷 입니다.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> 입니다.</param>
        /// <returns></returns>
        Task SendDataAsync(WebSocketIoResponse response, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 클라이언트가 채널(방)으로 접속합니다.
        /// </summary>
        /// <param name="channelKey">채널(방) 이름입니다.</param>
        Task JoinAsync(string channelKey);

        /// <summary>
        /// 채널(방)에서 클라이언트를 제거합니다.
        /// </summary>
        /// <param name="channelKey">채널(방) 이름입니다.</param>
        Task LeaveAsync(string channelKey);

        /// <summary>
        /// 모든 채널(방)에서 클라이언트를 제거합니다.
        /// </summary>
        Task LeaveAllAsync();

    }
}