using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Ntreev.AspNetCore.WebSocketIo.Authentication;

namespace Ntreev.AspNetCore.WebSocketIo.Filters
{
    public class AuthorizeAttributeTrackingFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthenticationHandlerProvider _authenticationHandlerProvider;

        private static readonly MethodInfo HandleAuthenticateAsyncMethod =
            typeof(WebSocketIoAuthenticationHandler).GetMethod("HandleAuthenticateAsync",
                BindingFlags.Instance | BindingFlags.NonPublic);

        public AuthorizeAttributeTrackingFilter(IAuthenticationHandlerProvider authenticationHandlerProvider)
        {
            _authenticationHandlerProvider = authenticationHandlerProvider;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.WebSockets.IsWebSocketRequest) return;

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null)
                throw new NullReferenceException(nameof(controllerActionDescriptor));

            var anonymousActionAttributes =
                controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
            if (anonymousActionAttributes.Length > 0) return;

            var controllerAttributes =
                controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
            var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);

            if ((controllerAttributes.Length + actionAttributes.Length) == 0) return;

            var handler = await _authenticationHandlerProvider.GetHandlerAsync(context.HttpContext,
                WebSocketIoDefaults.AuthenticationSchema);
            
            if (HandleAuthenticateAsyncMethod == null) 
                throw new NullReferenceException(nameof(HandleAuthenticateAsyncMethod));

            var resultTask = HandleAuthenticateAsyncMethod.Invoke(handler, new object[] { }) as Task<AuthenticateResult>;
            if (resultTask == null)
                throw new InvalidOperationException(nameof(resultTask));

            var result = resultTask.GetAwaiter().GetResult();


            if (result.Succeeded == false && result.None == false)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
