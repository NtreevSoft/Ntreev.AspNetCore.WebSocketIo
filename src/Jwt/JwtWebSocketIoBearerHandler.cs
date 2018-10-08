using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Ntreev.AspNetCore.WebSocketIo.Jwt
{
    [Obsolete("사용되지 않음. 삭제 예정")]
    public class JwtWebSocketIoBearerHandler : JwtBearerHandler
    {
        public JwtWebSocketIoBearerHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, IDataProtectionProvider dataProtection, ISystemClock clock) : base(options, logger, encoder, dataProtection, clock)
        {
        }

        private OpenIdConnectConfiguration _configuration;
        
        protected override Task<object> CreateEventsAsync()
        {
            return Task.FromResult((object)new JwtBearerEvents());
        }

        /// <summary>
        /// Searches the 'Authorization' header for a 'Bearer' token. If the 'Bearer' token is found, it is validated using <see cref="T:Microsoft.IdentityModel.Tokens.TokenValidationParameters" /> set in the options.
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token;
            Exception obj;
            AuthenticationFailedContext authenticationFailedContext;
            int num;
            try
            {
                var messageReceivedContext = new MessageReceivedContext(Context, Scheme, Options);
                await Events.MessageReceived(messageReceivedContext);
                if (messageReceivedContext.Result != null)
                    return messageReceivedContext.Result;
                token = messageReceivedContext.Token;
                if (string.IsNullOrEmpty(token))
                {
                    string header = Request.Headers["Authorization"];
                    if (string.IsNullOrEmpty(header))
                        return AuthenticateResult.NoResult();
                    if (header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        token = header.Substring("Bearer ".Length).Trim();
                    if (string.IsNullOrEmpty(token))
                        return AuthenticateResult.NoResult();
                }
                if (_configuration == null && Options.ConfigurationManager != null)
                {
                    var configurationAsync = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
                    _configuration = configurationAsync;
                }
                var validationParameters1 = Options.TokenValidationParameters.Clone();
                if (_configuration != null)
                {
                    var strArray = new string[1]
                    {
                        _configuration.Issuer
                    };
                    var validationParameters2 = validationParameters1;
                    var validIssuers = validationParameters1.ValidIssuers;
                    var obj1 = (validIssuers != null ? validIssuers.Concat(strArray) : null) ?? strArray;
                    validationParameters2.ValidIssuers = obj1;
                    var validationParameters3 = validationParameters1;
                    var issuerSigningKeys = validationParameters1.IssuerSigningKeys;
                    var securityKeys = (issuerSigningKeys != null ? issuerSigningKeys.Concat(_configuration.SigningKeys) : null) ?? _configuration.SigningKeys;
                    validationParameters3.IssuerSigningKeys = securityKeys;
                }
                List<Exception> exceptionList = null;
                foreach (var securityTokenValidator in Options.SecurityTokenValidators)
                {
                    if (securityTokenValidator.CanReadToken(token))
                    {
                        SecurityToken securityToken;
                        ClaimsPrincipal claimsPrincipal;
                        try
                        {
                            claimsPrincipal = securityTokenValidator.ValidateToken(token, validationParameters1, out securityToken);
                        }
                        catch (Exception ex)
                        {
                            //Logger.TokenValidationFailed(ex);
                            if (Options.RefreshOnIssuerKeyNotFound && Options.ConfigurationManager != null && ex is SecurityTokenSignatureKeyNotFoundException)
                                Options.ConfigurationManager.RequestRefresh();
                            if (exceptionList == null)
                                exceptionList = new List<Exception>(1);
                            exceptionList.Add(ex);
                            continue;
                        }
                        //Logger.TokenValidationSucceeded();
                        var validatedContext = new TokenValidatedContext(Context, Scheme, Options);
                        validatedContext.Principal = claimsPrincipal;
                        validatedContext.SecurityToken = securityToken;
                        var tokenValidatedContext = validatedContext;
                        await Events.TokenValidated(tokenValidatedContext);
                        if (tokenValidatedContext.Result != null)
                            return tokenValidatedContext.Result;
                        if (Options.SaveToken)
                            tokenValidatedContext.Properties.StoreTokens(new AuthenticationToken[1]
                            {
                                new AuthenticationToken()
                                {
                                    Name = "access_token",
                                    Value = token
                                }
                            });
                        tokenValidatedContext.Success();
                        return tokenValidatedContext.Result;
                    }
                }
                if (exceptionList == null)
                    return AuthenticateResult.Fail("No SecurityTokenValidator available for token: " + token ?? "[null]");
                authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
                {
                    Exception = exceptionList.Count == 1 ? exceptionList[0] : new AggregateException(exceptionList)
                };
                await Events.AuthenticationFailed(authenticationFailedContext);
                return authenticationFailedContext.Result == null ? AuthenticateResult.Fail(authenticationFailedContext.Exception) : authenticationFailedContext.Result;
            }
            catch (Exception ex)
            {
                obj = ex;
                num = 1;
            }
            if (num == 1)
            {
                var ex = obj;
                //Logger.ErrorProcessingMessage(ex);
                authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
                {
                    Exception = ex
                };
                await Events.AuthenticationFailed(authenticationFailedContext);
                if (authenticationFailedContext.Result != null)
                    return authenticationFailedContext.Result;
                var source = obj as Exception;
                if (source == null)
                    throw obj;
                ExceptionDispatchInfo.Capture(source).Throw();
                authenticationFailedContext = null;
            }

            return null;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (Context.WebSockets.IsWebSocketRequest)
            {
                //await base.InitializeAsync(Scheme, context);
                return;
            }

            //await base.HandleChallengeAsync(properties);

            var authenticateResult = await HandleAuthenticateOnceSafeAsync();
            var eventContext = new JwtBearerChallengeContext(Context, Scheme, Options, properties)
            {
                AuthenticateFailure = authenticateResult?.Failure
            };
            if (Options.IncludeErrorDetails && eventContext.AuthenticateFailure != null)
            {
                eventContext.Error = "invalid_token";
                eventContext.ErrorDescription = CreateErrorDescription(eventContext.AuthenticateFailure);
            }
            await Events.Challenge(eventContext);
            if (eventContext.Handled)
                return;

            Response.StatusCode = 401;

            if (string.IsNullOrEmpty(eventContext.Error) && string.IsNullOrEmpty(eventContext.ErrorDescription) && string.IsNullOrEmpty(eventContext.ErrorUri))
            {
                Response.Headers.Append("WWW-Authenticate", (StringValues)Options.Challenge);
            }
            else
            {
                var stringBuilder = new StringBuilder(Options.Challenge);
                if (Options.Challenge.IndexOf(" ", StringComparison.Ordinal) > 0)
                    stringBuilder.Append(',');
                if (!string.IsNullOrEmpty(eventContext.Error))
                {
                    stringBuilder.Append(" error=\"");
                    stringBuilder.Append(eventContext.Error);
                    stringBuilder.Append("\"");
                }
                if (!string.IsNullOrEmpty(eventContext.ErrorDescription))
                {
                    if (!string.IsNullOrEmpty(eventContext.Error))
                        stringBuilder.Append(",");
                    stringBuilder.Append(" error_description=\"");
                    stringBuilder.Append(eventContext.ErrorDescription);
                    stringBuilder.Append('"');
                }
                if (!string.IsNullOrEmpty(eventContext.ErrorUri))
                {
                    if (!string.IsNullOrEmpty(eventContext.Error) || !string.IsNullOrEmpty(eventContext.ErrorDescription))
                        stringBuilder.Append(",");
                    stringBuilder.Append(" error_uri=\"");
                    stringBuilder.Append(eventContext.ErrorUri);
                    stringBuilder.Append('"');
                }

                //Response.Headers.Append("WWW-Authenticate", (StringValues)stringBuilder.ToString());
            }
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        private static string CreateErrorDescription(Exception authFailure)
        {
            IEnumerable<Exception> exceptions;
            if (authFailure is AggregateException)
                exceptions = (authFailure as AggregateException).InnerExceptions;
            else
                exceptions = new Exception[1]
                {
                    authFailure
                };
            var stringList = new List<string>();
            foreach (var exception in exceptions)
            {
                if (exception is SecurityTokenInvalidAudienceException)
                    stringList.Add("The audience is invalid");
                else if (exception is SecurityTokenInvalidIssuerException)
                    stringList.Add("The issuer is invalid");
                else if (exception is SecurityTokenNoExpirationException)
                    stringList.Add("The token has no expiration");
                else if (exception is SecurityTokenInvalidLifetimeException)
                    stringList.Add("The token lifetime is invalid");
                else if (exception is SecurityTokenNotYetValidException)
                    stringList.Add("The token is not valid yet");
                else if (exception is SecurityTokenExpiredException)
                    stringList.Add("The token is expired");
                else if (exception is SecurityTokenSignatureKeyNotFoundException)
                    stringList.Add("The signature key was not found");
                else if (exception is SecurityTokenInvalidSignatureException)
                    stringList.Add("The signature is invalid");
            }
            return string.Join("; ", stringList);
        }
    }
}