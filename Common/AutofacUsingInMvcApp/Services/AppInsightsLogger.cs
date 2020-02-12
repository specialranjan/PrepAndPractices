using System;
using System.Diagnostics;

namespace AutofacUsingInMvcApp.Services
{
    public class AppInsightsLogger: ITelemetryLogger
    {
        private readonly string applicationInsightsKey;
        private readonly string applicationInsightsName;
        public AppInsightsLogger(string appInsightsKey, string appInsightsName)
        {
            this.applicationInsightsKey = appInsightsKey;
            this.applicationInsightsName = appInsightsName;
        }
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