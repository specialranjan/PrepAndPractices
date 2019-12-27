
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using CredentialBasedTokenAuthDemo.Api.Models;

namespace CredentialBasedTokenAuthDemo.Api.Controllers
{
    public class IdentityController : ApiController
    {
        public IEnumerable<IdentityClaims> Get()
        {
            var principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            return from c in principal.Claims
                   select new IdentityClaims
                   {
                       Type = c.Type,
                       Value = c.Value
                   };
        }
    }
}
