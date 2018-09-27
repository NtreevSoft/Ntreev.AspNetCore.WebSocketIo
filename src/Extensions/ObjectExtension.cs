using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ntreev.AspNetCore.WebSocketIo.Extensions
{
    internal static class ObjectExtension
    {
        static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        internal static string ToJson(this object obj, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(obj, settings ?? jsonSerializerSettings);
        } 
    }
}