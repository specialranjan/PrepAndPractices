using System;

namespace AutofacUsingInMvcApp.Services
{
    public interface ITelemetryLogger
    {
        void LogException(Exception exception);
        void LogError(string errorMessage);
        void LogInformation(string message);
    }
}
