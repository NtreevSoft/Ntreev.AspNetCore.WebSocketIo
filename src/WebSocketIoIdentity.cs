using System.Security.Principal;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketIoIdentity : IIdentity
    {
        private readonly IIdentity _identity;

        public WebSocketIoIdentity(IIdentity identity, bool isAuthenticated)
        {
            _identity = identity;
            IsAuthenticated = isAuthenticated;
        }

        public string AuthenticationType => _identity.AuthenticationType;
        public bool IsAuthenticated { get; }
        public string Name => _identity.Name;
    }
}