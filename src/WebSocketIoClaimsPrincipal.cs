using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Ntreev.AspNetCore.WebSocketIo
{
    [Obsolete("사용하지 않음. 제거 예정")]
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
