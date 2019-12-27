using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Api.Auth.TokenBased.Infrastructure.Filters.Authentication;

namespace Web.Api.Auth.TokenBased.Controllers
{
    [TokenAuthentication]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
