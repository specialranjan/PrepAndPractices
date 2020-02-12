using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutofacUsingInMvcApp.Services
{
    public class TestService : ITestService
    {
        private readonly ILogger logger;
        public TestService(ILogger logger)
        {
            this.logger = logger;
        }
        public int TestMethod(int a, int b)
        {
            this.logger.LogInformation("TestService.TestMethod is invoked.");
            return a + b;
        }
    }
}