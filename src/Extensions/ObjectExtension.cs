using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    /// <summary>
    /// ������Ʈ�� Ȯ�� �޼��� Ŭ���� �Դϴ�.
    /// </summary>
    internal static class ObjectExtension
    {
        /// <summary>
        /// �⺻���� JSON ����ȭ �ɼ� �Դϴ�.
        /// </summary>
        static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// JSON ����ȭ �մϴ�.
        /// </summary>
        /// <param name="obj">��� ��ü �Դϴ�.</param>
        /// <param name="settings">����ȭ �ɼ��Դϴ�. �� ���� �������� ������ <see cref="jsonSerializerSettings"/> ���� �̿��մϴ�.</param>
        /// <returns></returns>
        internal static string ToJson(this object obj, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(obj, settings ?? jsonSerializerSettings);
        } 
    }
}