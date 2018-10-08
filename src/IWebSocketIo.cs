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
    /// ������ ����� �����ϴ� �������̽� �Դϴ�.
    /// </summary>
    public interface IWebSocketIo
    {
        /// <summary>
        /// �������� Id
        /// </summary>
        Guid SocketId { get; }

        /// <summary>
        /// <see cref="WebSocket"/> ��ü
        /// </summary>
        WebSocket Socket { get; }

        /// <summary>
        /// ��ε�ĳ��Ʈ �޽��� ���� �Դϴ�.
        /// </summary>
        IBroadcastBuilder Broadcast { get; }

        /// <summary>
        /// ��� �޽����� ������ ���� ���� �Դϴ�.
        /// </summary>
        IPrivateBuilder Private { get; }

        /// <summary>
        /// �������� �Ҽӵ� ä��(��) ��� �Դϴ�.
        /// </summary>
        IList<string> JoinedChannels { get; }

        /// <summary>
        /// ������ ������ ����ų� ����ڰ� ������ �߻��ϴ� �̺�Ʈ �Դϴ�.
        /// </summary>
        event EventHandler<WebSocketIoEventArgs> Leaved;

        /// <summary>
        /// ������ ������ ����ų� ����ڰ� ������ ���� �߻��ϴ� �̺�Ʈ �Դϴ�.
        /// </summary>
        event EventHandler Disconnecting;

        /// <summary>
        /// <see cref="Leaved"/> �̺�Ʈ�� �߻��մϴ�.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">�Ű�����</param>
        void OnLeaved(object sender, WebSocketIoEventArgs args);

        /// <summary>
        /// <see cref="Disconnecting"/> �̺�Ʈ�� �߻��մϴ�.
        /// </summary>
        /// <param name="sender">Sender</param>
        void OnDisconnecting(object sender);

        /// <summary>
        /// Ŭ���̾�Ʈ���� �����͸� �����մϴ�.
        /// </summary>
        /// <param name="data">������</param>
        /// <param name="endOfMessage">�޽����� ������ �ƴ��� �����Դϴ�.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> �Դϴ�.</param>
        Task SendDataAsync(string data, bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ŭ���̾�Ʈ���� ������ ��ü�� �����մϴ�.
        /// </summary>
        /// <param name="obj">������ ��ü</param>
        /// <param name="endOfMessage">�޽����� ������ �ƴ��� �����Դϴ�.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> �Դϴ�.</param>
        /// <returns></returns>
        Task SendDataAsync(object obj, bool endOfMessage = true, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ŭ���̾�Ʈ���� ������ ���� ��Ŷ�� �����մϴ�.
        /// </summary>
        /// <param name="response">������ ���� ��Ŷ �Դϴ�.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> �Դϴ�.</param>
        /// <returns></returns>
        Task SendDataAsync(WebSocketIoResponse response, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Ŭ���̾�Ʈ�� ä��(��)���� �����մϴ�.
        /// </summary>
        /// <param name="channelKey">ä��(��) �̸��Դϴ�.</param>
        Task JoinAsync(string channelKey);

        /// <summary>
        /// ä��(��)���� Ŭ���̾�Ʈ�� �����մϴ�.
        /// </summary>
        /// <param name="channelKey">ä��(��) �̸��Դϴ�.</param>
        Task LeaveAsync(string channelKey);

        /// <summary>
        /// ��� ä��(��)���� Ŭ���̾�Ʈ�� �����մϴ�.
        /// </summary>
        Task LeaveAllAsync();

    }
}