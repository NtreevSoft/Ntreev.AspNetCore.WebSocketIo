using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ntreev.AspNetCore.WebSocketIo
{
    /// <summary>
    /// �������� �����ϴ� �Ŵ��� �������̽� �Դϴ�.
    /// </summary>
    public interface IWebSocketIoConnectionManager
    {
        /// <summary>
        /// �������� �߰� �մϴ�.
        /// </summary>
        /// <param name="guid">Socket Id</param>
        /// <param name="socket"><see cref="IWebSocketIo"/> ��ü �Դϴ�.</param>
        void Add(Guid guid, IWebSocketIo socket);

        /// <summary>
        /// �������� �����ɴϴ�. �������� ������ null �� ��ȯ �մϴ�.
        /// </summary>
        /// <param name="guid">Socket Id</param>
        IWebSocketIo GetOrDefault(Guid guid);

        /// <summary>
        /// �������� �����մϴ�.
        /// </summary>
        /// <param name="guid">Socket Id</param>
        Task RemoveAsync(Guid guid);

        /// <summary>
        /// �������� ä��(��)�� �߰��մϴ�.
        /// </summary>
        /// <param name="key">ä��(��) Ű �Դϴ�.</param>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> ��ü �Դϴ�.</param>
        Task JoinAsync(string key, IWebSocketIo webSocketIo);

        /// <summary>
        /// �������� ä��(��)���� �����մϴ�.
        /// </summary>
        /// <param name="key">ä��(��) Ű �Դϴ�.</param>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> ��ü �Դϴ�.</param>
        Task LeaveAsync(string key, IWebSocketIo webSocketIo);

        /// <summary>
        /// �������� ��� ä�ΰ� ���� ��Ͽ��� �����մϴ�.
        /// </summary>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> ��ü �Դϴ�.</param>
        Task LeaveAllAsync(IWebSocketIo webSocketIo);

        /// <summary>
        /// �������� ���ҽ��� �����մϴ�.
        /// </summary>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> ��ü �Դϴ�.</param>
        Task DisposeAsync(IWebSocketIo webSocketIo);

        /// <summary>
        /// ä��(��)�� ��� �������� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="key">ä��(��) Ű �Դϴ�.</param>
        IEnumerable<IWebSocketIo> GetClientsInChannel(string key);

        /// <summary>
        /// ��� �������� ��ȯ�մϴ�.
        /// </summary>
        IEnumerable<IWebSocketIo> GetAll();

        /// <summary>
        /// �������� ����Ǹ� �߻��ϴ� �̺�Ʈ �Դϴ�.
        /// </summary>
        event EventHandler<IWebSocketIo> Connected;

        /// <summary>
        /// �������� ���� �����Ǹ� �߻��ϴ� �̺�Ʈ �Դϴ�.
        /// </summary>
        event EventHandler<IWebSocketIo> Disconnected;

        /// <summary>
        /// <see cref="Connected"/> �̺�Ʈ�� �߻��մϴ�.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> ��ü �Դϴ�.</param>
        void OnConnected(object sender, IWebSocketIo webSocketIo);

        /// <summary>
        /// <see cref="Disconnected"/> �̺�Ʈ�� �߻��մϴ�.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="webSocketIo"><see cref="IWebSocketIo"/> ��ü �Դϴ�.</param>
        void OnDisconnected(object sender, IWebSocketIo webSocketIo);
    }
}