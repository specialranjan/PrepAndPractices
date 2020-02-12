using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AutofacUsingInMvcApp.Services
{
    public class DebugLogger : ILogger
    {
        public void LogError(string errorMessage)
        {
            Debug.WriteLine(errorMessage);
        }

        public void LogException(Exception exception)
        {
            Debug.WriteLine(exception.Message);
        }

        public void LogInformation(string message)
        {
            Debug.WriteLine(message);
        }
    }
}