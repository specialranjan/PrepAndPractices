using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Common.Logging.AppInsights
{
    public interface ITelemetryHelper
    {
        bool AutoForcedFlushEnabled { get; set; }

        /// <summary>
        /// Track a metric
        /// </summary>
        /// <param name="name">Metric name</param>
        /// <param name="value">Metric value</param>
        /// <param name="component">Component name (Service and method name)</param>
        /// <param name="properties">Named string values you can use to classify and filter metrics</param>
        void TrackMetric(string name, double value, string component = null, Dictionary<string, string> properties = null, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Track information about external dependency call in the application.
        /// </summary>
        /// <param name="dependencyName">External dependency name.</param>
        /// <param name="commandName">Dependency call command name.</param>
        /// <param name="startTime">The time when the dependency was called.</param>
        /// <param name="duration">The time taken by the external dependency to handle the call.</param>
        /// <param name="success">True if the dependency call was handled successfully.</param>
        void TrackDependency(string dependencyName, string commandName, DateTimeOffset startTime, TimeSpan duration, bool success, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Track information about the page viewed in the application.
        /// </summary>
        /// <param name="name">Name of the page.</param>
        void TrackPageView(string name, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Track information about a request handled by the application.
        /// </summary>
        /// <param name="name">The request name.</param>
        /// <param name="startTime">The time when the page was requested.</param>
        /// <param name="duration">The time taken by the application to handle the request.</param>
        /// <param name="responseCode">The response status code.</param>
        /// <param name="success">True if the request was handled successfully by the application.</param>
        void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Track a metric for an event
        /// </summary>
        /// <param name="name">A name for the event</param>
        /// <param name="eventId">Unique event id</param>
        /// <param name="component">Component name (Service and method name)</param>
        /// <param name="metrics">Measurements associated with this event</param>
        /// <param name="properties">Named string values you can use to search and classify events</param>
        void TrackEvent(string name, Dictionary<string, double> metrics, string eventId = null, string component = null, Dictionary<string, string> properties = null, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "");

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
        void TrackException(Exception thrownException, string eventId = null, string errorId = null, string message = null, string component = null, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Track a trace message
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="sev">Trace severity level <see cref="Severity"/></param>
        /// <param name="eventId">Unique event id (ex. Guid)</param>
        /// <param name="component">Component name (Service and method name)</param>
        /// <param name="properties">Named string values you can use to search and classify events</param>
        void TrackTrace(string message, Severity sev, string eventId = null, string component = null, Dictionary<string, string> properties = null, [CallerFilePath] string sourceFile = "", [CallerLineNumber] int sourceLine = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Flushes the in-memory buffer.
        /// </summary>
        void Flush();
    }
}
