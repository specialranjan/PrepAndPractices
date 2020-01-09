namespace Employee.App.Api.Infrastructure.Filters.Authentication
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;
    using Employee.App.Common.Helpers;

    public class TokenAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private string authenticationScheme = "Bearer";
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {                
                return;
            }
            string authenticationKey = null;
            IPrincipal principal = AuthorizationHelper.ValidateJwtToken("", authorization.Parameter, true);
            if (principal == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(
                ((ClaimsIdentity)principal.Identity).Claims.FirstOrDefault(c => c.Type == "UserId")
                ?.Value))
            {
                return;
            }

            System.Web.HttpContext.Current.User = principal;
            Thread.CurrentPrincipal = principal;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue(this.authenticationScheme);
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }
    }
}