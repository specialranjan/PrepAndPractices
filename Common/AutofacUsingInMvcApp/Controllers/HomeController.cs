using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutofacUsingInMvcApp.Services;

namespace AutofacUsingInMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger logger;
        private readonly ITestService testService;
        private readonly ITelemetryLogger telemetryLogger;

        public HomeController(ILogger logger, ITestService testService, ITelemetryLogger telemetryLogger)
        {
            this.logger = logger;
            this.testService = testService;
            this.telemetryLogger = telemetryLogger;
        }
        public ActionResult Index()
        {
            var result = this.testService.TestMethod(4, 3);
            this.logger.LogInformation(string.Format("Test Service - Test Method result is: {0}", result));
            this.telemetryLogger.LogInformation(string.Format("Test Service - Test Method result is: {0} using Telemetry Logger", result));
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}