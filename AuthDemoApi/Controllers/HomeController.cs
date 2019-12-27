using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CredentialBasedTokenAuthDemo.Api.Infrastructure.Filters.Authentication;

namespace CredentialBasedTokenAuthDemo.Api.Controllers
{
    [TokenAuthentication]
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("menus")]
        public async Task<HttpResponseMessage> GetMenus()
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
