using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// 웹소켓을 관리하는 매니저 인터페이스 입니다.
    /// </summary>
    public interface IWebSocketIoConnectionManager
    {
        /// <summary>
        /// 웹소켓을 추가 합니다.
        /// </summary>
        /// <param name="guid">Socket Id</param>
        /// <param name="socket"><see cref="IWebSocketIo"/> 객체 입니다.</param>
        void Add(Guid guid, IWebSocketIo socket);

        /// <summary>
        /// 웹소켓을 가져옵니다. 웹소켓이 없으면 null 을 반환 합니다.
        /// </summary>
        /// <param name="guid">Socket Id</param>
        IWebSocketIo GetOrDefault(Guid guid);

        /// <summary>
        /// 웹소켓을 제거합니다.
        /// </summary>
        /// <param name="guid">Socket Id</param>
        void Remove(Guid guid);

        /// <summary>
        /// 웹소켓을 채널(방)에 추가합니다.
        /// </summary>
        /// <param name="key">채널(방) 키 입니다.</param>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> 객체 입니다.</param>
        Task JoinAsync(string key, IWebSocketIo webSocketIo);

        /// <summary>
        /// 웹소켓을 채널(방)에서 제거합니다.
        /// </summary>
        /// <param name="key">채널(방) 키 입니다.</param>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> 객체 입니다.</param>
        Task LeaveAsync(string key, IWebSocketIo webSocketIo);

        /// <summary>
        /// 웹소켓을 모든 채널과 관리 목록에서 제거합니다.
        /// </summary>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> 객체 입니다.</param>
        Task LeaveAllAsync(IWebSocketIo webSocketIo);

        /// <summary>
        /// 웹소켓의 리소스를 해제합니다.
        /// </summary>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> 객체 입니다.</param>
        Task DisposeAsync(IWebSocketIo webSocketIo);

        /// <summary>
        /// 채널(방)의 모든 웹소켓을 반환합니다.
        /// </summary>
        /// <param name="key">채널(방) 키 입니다.</param>
        IEnumerable<IWebSocketIo> GetClientsInChannel(string key);

        /// <summary>
        /// 모든 웹소켓을 반환합니다.
        /// </summary>
        IEnumerable<IWebSocketIo> GetAll();

        /// <summary>
        /// 웹소켓이 연결되면 발생하는 이벤트 입니다.
        /// </summary>
        event EventHandler<IWebSocketIo> Connected;

        /// <summary>
        /// 웹소켓이 연결 해제되면 발생하는 이벤트 입니다.
        /// </summary>
        event EventHandler<IWebSocketIo> Disconnected;

        /// <summary>
        /// <see cref="Connected"/> 이벤트를 발생합니다.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> 객체 입니다.</param>
        void OnConnected(object sender, IWebSocketIo webSocketIo);

        /// <summary>
        /// <see cref="Disconnected"/> 이벤트를 발생합니다.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> 객체 입니다.</param>
        void OnDisconnected(object sender, IWebSocketIo webSocketIo);
    }
}