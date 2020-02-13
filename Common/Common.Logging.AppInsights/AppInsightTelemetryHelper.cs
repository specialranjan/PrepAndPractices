using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;


namespace Common.Logging.AppInsights
{
    public class AppInsightTelemetryHelper : ITelemetryHelper
    {
        public bool AutoForcedFlushEnabled { get; set; }

        public AppInsightTelemetryHelper(bool forcedFlush = false)
        {
            _telemetryClient = new TelemetryClient();
            AutoForcedFlushEnabled = forcedFlush;
        }

        public AppInsightTelemetryHelper(string appInsightsInstrumentationKey, bool forceFlush = false) : this(forceFlush)
        {
            InitTelemetry(appInsightsInstrumentationKey);
        }

        private const string ComponentKey = "component";
        private const string EventIdKey = "eventid";
        private const string ErrorIdKey = "errorid";
        private const string MessageKey = "message";

        private readonly TelemetryClient _telemetryClient;

        public TelemetryContext Context => _telemetryClient.Context;

        /// <summary>
        /// Initializes the telemetry.
        /// </summary>
        /// <param name="appInsightsInstrumentationKey">The application insights instrumentation key.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Instrumentation key cannot be empty</exception>
        public void InitTelemetry(string appInsightsInstrumentationKey)
        {
            if (string.IsNullOrWhiteSpace(appInsightsInstrumentationKey))
            {
                throw new ArgumentException("Instrumentation key cannot be empty", nameof(appInsightsInstrumentationKey));
            }

            //Setup our telemetry client to be able to call
            TelemetryConfiguration.Active.InstrumentationKey = appInsightsInstrumentationKey;
            _telemetryClient.InstrumentationKey = TelemetryConfiguration.Active.InstrumentationKey;
        }

        /// <summary>
        /// Track a metric
        /// </summary>
        /// <param name="name">Metric name</param>
        /// <param name="value">Metric value</param>
        /// <param name="component">Component name (Service and method name)</param>
        /// <param name="properties">Named string values you can use to classify and filter metrics</param>
        public void TrackMetric(string name, double value, string component = null, Dictionary<string, string> properties = null, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            if (properties == null)
            { properties = new Dictionary<string, string>(); }

            var localDic = new Dictionary<string, string>(properties);

            if (!string.IsNullOrWhiteSpace(component))
            {
                localDic.Add(ComponentKey, component);
            }

            _telemetryClient.TrackMetric(name, value, localDic);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track a metric
        /// </summary>
        /// <param name="metricEvent">Metric event to track.</param>
        public void TrackMetric(MetricTelemetry metricEvent, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            _telemetryClient.TrackMetric(metricEvent);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track information about external dependency call in the application.
        /// </summary>
        /// <param name="dependencyName">External dependency name.</param>
        /// <param name="commandName">Dependency call command name.</param>
        /// <param name="startTime">The time when the dependency was called.</param>
        /// <param name="duration">The time taken by the external dependency to handle the call.</param>
        /// <param name="success">True if the dependency call was handled successfully.</param>
        public void TrackDependency(string dependencyName, string commandName, DateTimeOffset startTime, TimeSpan duration, bool success, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            _telemetryClient.TrackDependency(dependencyName, commandName, startTime, duration, success);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track information about external dependency call in the application.
        /// </summary>
        /// <param name="dependencyTelemetry">Dependency to track.</param>
        public void TrackDependency(DependencyTelemetry dependencyTelemetry, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            _telemetryClient.TrackDependency(dependencyTelemetry);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track information about the page viewed in the application.
        /// </summary>
        /// <param name="pageViewTelemetry">Page view to track.</param>
        public void TrackPageView(PageViewTelemetry pageViewTelemetry, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            _telemetryClient.TrackPageView(pageViewTelemetry);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track information about the page viewed in the application.
        /// </summary>
        /// <param name="name">Name of the page.</param>
        public void TrackPageView(string name, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            _telemetryClient.TrackPageView(name);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track information about a request handled by the application.
        /// </summary>
        /// <param name="requestTelemetry">Request to track.</param>
        public void TrackRequest(RequestTelemetry requestTelemetry, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            _telemetryClient.TrackRequest(requestTelemetry);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track information about a request handled by the application.
        /// </summary>
        /// <param name="name">The request name.</param>
        /// <param name="startTime">The time when the page was requested.</param>
        /// <param name="duration">The time taken by the application to handle the request.</param>
        /// <param name="responseCode">The response status code.</param>
        /// <param name="success">True if the request was handled successfully by the application.</param>
        public void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            _telemetryClient.TrackRequest(name, startTime, duration, responseCode, success);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track a metric for an event
        /// </summary>
        /// <param name="eventTelemetry">Event to track.</param>
        public void TrackEvent(EventTelemetry eventTelemetry, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            _telemetryClient.TrackEvent(eventTelemetry);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track a metric for an event
        /// </summary>
        /// <param name="name">A name for the event</param>
        /// <param name="eventId">Unique event id</param>
        /// <param name="component">Component name (Service and method name)</param>
        /// <param name="metrics">Measurements associated with this event</param>
        /// <param name="properties">Named string values you can use to search and classify events</param>
        public void TrackEvent(string name, Dictionary<string, double> metrics, string eventId = null, string component = null, Dictionary<string, string> properties = null, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            var localDic = new Dictionary<string, string>(properties);
            if (!string.IsNullOrWhiteSpace(component))
            {
                localDic.Add(ComponentKey, component);
            }

            if (!string.IsNullOrWhiteSpace(eventId))
            {
                localDic.Add(EventIdKey, eventId);
            }

            _telemetryClient.TrackEvent(name, localDic, metrics);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track an exception
        /// </summary>
        /// <param name="exceptionTelemetry">Exception to track.</param>
        public void TrackException(ExceptionTelemetry exceptionTelemetry, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            _telemetryClient.TrackException(exceptionTelemetry);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track an exception
        /// </summary>
        /// <param name="thrownException">The exception to log</param>
        /// <param name="eventId">Unique event id</param>
        /// <param name="errorId">Error code</param>
        /// <param name="message">Additional error message to store with the exception</param>
        /// <param name="component">Component name (Service and method name)</param>
        /// <param name="properties">Named string values you can use to classify and search for this exception</param>
        /// <param name="metrics">Additional values associated with this exception</param>
        public void TrackException(Exception thrownException, string eventId = null, string errorId = null,
            string message = null, string component = null, Dictionary<string, string> properties = null,
            Dictionary<string, double> metrics = null, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            var localDic = new Dictionary<string, string>(properties);
            if (!string.IsNullOrWhiteSpace(component))
            {
                localDic.Add(ComponentKey, component);
            }

            if (!string.IsNullOrWhiteSpace(errorId))
            {
                localDic.Add(ErrorIdKey, errorId);
            }

            if (!string.IsNullOrWhiteSpace(eventId))
            {
                localDic.Add(EventIdKey, eventId);
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                localDic.Add(MessageKey, message);
            }

            _telemetryClient.TrackException(thrownException, localDic, metrics);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track a trace message
        /// </summary>
        /// <param name="traceTelemetry">Trace to track.</param>
        public void TrackTrace(TraceTelemetry traceTelemetry, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            _telemetryClient.TrackTrace(traceTelemetry);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Track a trace message
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="sev">Trace severity level <see cref="Severity" /></param>
        /// <param name="eventId">Unique event id (ex. Guid)</param>
        /// <param name="component">Component name (Service and method name)</param>
        /// <param name="properties">Named string values you can use to search and classify events</param>
        public void TrackTrace(string message, Severity sev, string eventId = null, string component = null,
            Dictionary<string, string> properties = null, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "")
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            var localDic = new Dictionary<string, string>(properties);

            if (!string.IsNullOrWhiteSpace(component))
            {
                localDic.Add(ComponentKey, component);
            }

            if (!string.IsNullOrWhiteSpace(eventId))
            {
                localDic.Add(EventIdKey, eventId);
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                localDic.Add(MessageKey, message);
            }

            SeverityLevel severity;
            switch (sev)
            {
                case Severity.Critical:
                    severity = SeverityLevel.Critical;
                    break;

                case Severity.Error:
                    severity = SeverityLevel.Error;
                    break;

                case Severity.Information:
                    severity = SeverityLevel.Information;
                    break;

                case Severity.Verbose:
                    severity = SeverityLevel.Verbose;
                    break;

                case Severity.Warning:
                    severity = SeverityLevel.Warning;
                    break;

                default:
                    severity = SeverityLevel.Verbose;
                    break;
            }
            _telemetryClient.TrackTrace(message, severity, localDic);
            if (AutoForcedFlushEnabled)
            {
                Flush();
            }
        }

        /// <summary>
        /// Flushes the in-memory buffer.
        /// </summary>
        public void Flush()
        {
            _telemetryClient.Flush();
        }

    }
}
