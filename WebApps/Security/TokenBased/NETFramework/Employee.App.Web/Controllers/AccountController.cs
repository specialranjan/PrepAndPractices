namespace Employee.App.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OpenIdConnect;

    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!this.Request.IsAuthenticated)
            {
                this.HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public ActionResult UnauthorizeAccess()
        {
            return this.View("UnauthorizeAccess");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task SignOut()
        {
            this.Session.Clear();
            this.Session.Abandon();
            HttpCookie myCookie = new HttpCookie("ASP.NET_SessionId");
            myCookie.HttpOnly = true;
            myCookie.Secure = true;
            this.Response.Cookies.Add(myCookie);
            string callbackUrl = this.Url.Action("SignOutCallback", "Account", null, this.Request.Url.Scheme);
            this.HttpContext.GetOwinContext().Authentication.SignOut(new AuthenticationProperties { RedirectUri = callbackUrl }, OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}