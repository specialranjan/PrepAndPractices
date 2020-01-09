namespace Employee.App.Web.Controllers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Employee.App.Common;
    using Employee.App.Web.Infrastructure;

    [Authorize]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            GetEmployeeDirectoryList().GetAwaiter();

            return View();
        }

        private async Task GetEmployeeDirectoryList()
        {
            WebHelper webHelper = new WebHelper();
            var accessToken = await webHelper.GetTokens().ConfigureAwait(false);

            HttpClientWrapper httpClientWrapper = new HttpClientWrapper();
            var requestUri = new System.Uri("https://localhost:44333/api/employee/directory/allcontacts");
            HttpResponseMessage response = await httpClientWrapper.GetAsync(requestUri, accessToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return;
            }
        }
    }
}