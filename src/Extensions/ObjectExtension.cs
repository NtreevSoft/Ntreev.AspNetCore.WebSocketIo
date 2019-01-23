using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    /// <summary>
    /// 오브젝트의 확장 메서드 클래스 입니다.
    /// </summary>
    internal static class ObjectExtension
    {
        /// <summary>
        /// 기본적인 JSON 직렬화 옵션 입니다.
        /// </summary>
        static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// JSON 직렬화 합니다.
        /// </summary>
        /// <param name="obj">대상 객체 입니다.</param>
        /// <param name="settings">직렬화 옵션입니다. 이 값이 지정되지 않으면 <see cref="jsonSerializerSettings"/> 값을 이용합니다.</param>
        /// <returns></returns>
        internal static string ToJson(this object obj, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(obj, settings ?? jsonSerializerSettings);
        } 
    }
}