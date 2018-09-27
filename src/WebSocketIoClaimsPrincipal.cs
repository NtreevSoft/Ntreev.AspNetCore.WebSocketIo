using System.Security.Claims;
using System.Security.Principal;

namespace Ntreev.AspNetCore.WebSocketIo
{
    public class WebSocketIoClaimsPrincipal : ClaimsPrincipal
    {
        private readonly IPrincipal _principal;
        private IIdentity _identity;

        public WebSocketIoClaimsPrincipal(IPrincipal principal) : base(principal)
        {
            _principal = principal;
        }

        public WebSocketIoClaimsPrincipal(IPrincipal principal, bool isAuthentication) : base(principal)
        {
            _principal = principal;
            _identity = new WebSocketIoIdentity(principal.Identity, isAuthentication);
        }

        public override IIdentity Identity => _identity;
    }
}
