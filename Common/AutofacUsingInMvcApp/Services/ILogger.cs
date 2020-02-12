using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AutofacUsingInMvcApp.Services
{
    public interface ILogger
    {
        void LogException(Exception exception);
        void LogError(string errorMessage);
        void LogInformation(string message);
    }
}