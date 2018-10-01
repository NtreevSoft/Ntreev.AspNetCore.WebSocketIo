using System;
using System.Collections.Generic;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketIoPacket
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public IDictionary<string, string> Headers = new Dictionary<string, string>(StringComparer.InvariantCulture);
        public object Data { get; set; }
    }
}