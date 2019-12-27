using System.Web.Http;
using System.Web.Http.Controllers;

namespace CredentialBasedTokenAuthDemo.Api.Infrastructure.Filters.Authentication
{
    public class AuthorizationFilterAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return true;
        }
    }
}