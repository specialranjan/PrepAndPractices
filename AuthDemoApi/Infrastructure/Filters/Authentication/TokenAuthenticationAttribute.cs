using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace CredentialBasedTokenAuthDemo.Api.Infrastructure.Filters.Authentication
{
    public class TokenAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        bool IFilter.AllowMultiple => false;
        private string authenticationScheme = "Bearer";

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Authorization header required", request);
                return;
            }

            if (!this.authenticationScheme.Equals(authorization.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid authorization scheme", request);
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Jwt Token Required", request);
                return;
            }

            IPrincipal principal = null;//this.HttpClientHelper.ValidateJwtToken(authorization.Parameter, true);
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("InvalidJwtToken", request);
                return;
            }
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            //var challenge = new AuthenticationHeaderValue(this.authenticationScheme);
            //context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return;
        }
    }
}