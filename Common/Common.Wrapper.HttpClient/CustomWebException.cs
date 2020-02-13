namespace Common.Wrapper.HttpClient
{
    using System;
    using System.Net;

    /// <summary>
    /// The custom web exception.
    /// </summary>
    public class CustomWebException : WebException
    {
        /// <summary>
        /// The URI.
        /// </summary>
        private readonly Uri uri;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWebException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="uri">
        /// The URI.
        /// </param>
        public CustomWebException(string message, Uri uri)
            : base(message)
        {
            this.uri = uri;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWebException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="statusCode">The status code.</param>
        public CustomWebException(string message, Uri uri, HttpStatusCode statusCode)
            : base(message)
        {
            this.uri = uri;
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWebException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        public CustomWebException(string message, Exception innerException, Uri requestUri)
            : base(message, innerException)
        {
            this.uri = requestUri;
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode? StatusCode { get; }
    }
}
