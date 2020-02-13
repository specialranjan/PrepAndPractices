namespace Common.Wrapper.HttpClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    ///     The exception handling helper.
    /// </summary>
    public class ExceptionHandlingHelper : IExceptionHandlingHelper
    {
        /// <summary>
        ///     The error mappings.
        /// </summary>
        private readonly Lazy<IReadOnlyDictionary<HttpStatusCode, Func<HttpResponseMessage, Uri, Task<WebException>>>> errorMappings;

        /// <summary>
        /// The custom HTTP status codes.
        /// </summary>
        private readonly List<HttpStatusCode> customHttpStatusCodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingHelper"/> class.
        /// </summary>
        public ExceptionHandlingHelper()
        {
            this.errorMappings = new Lazy<IReadOnlyDictionary<HttpStatusCode, Func<HttpResponseMessage, Uri, Task<WebException>>>>(
                    GetDefaultErrorMappings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingHelper"/> class.
        /// </summary>
        /// <param name="statusCodesList">The status codes list.</param>
        public ExceptionHandlingHelper(List<HttpStatusCode> statusCodesList)
        {
            this.customHttpStatusCodes = statusCodesList;
            this.errorMappings = new Lazy<IReadOnlyDictionary<HttpStatusCode, Func<HttpResponseMessage, Uri, Task<WebException>>>>(
                    this.GetCustomErrorMappings);
        }

        /// <summary>
        /// Gets the error mappings.
        /// </summary>
        /// <returns>The error mappings.</returns>
        public IReadOnlyDictionary<HttpStatusCode, Func<HttpResponseMessage, Uri, Task<WebException>>> GetErrorMappings()
        {
            return this.errorMappings.Value;
        }

        /// <summary>
        /// The get web exception message async.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private static async Task<string> GetWebExceptionMessageAsync(HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// The get default error mappings.
        /// </summary>
        /// <returns>
        /// The <see cref="IReadOnlyDictionary"/>.
        /// </returns>
        private static IReadOnlyDictionary<HttpStatusCode, Func<HttpResponseMessage, Uri, Task<WebException>>> GetDefaultErrorMappings()
        {
            var mappings =
                new Dictionary<HttpStatusCode, Func<HttpResponseMessage, Uri, Task<WebException>>>();
            mappings.Add(
                HttpStatusCode.BadRequest,
                async (response, uri) => new CustomWebException(
                    await GetWebExceptionMessageAsync(response).ConfigureAwait(false),
                    uri));
            mappings.Add(
                HttpStatusCode.ServiceUnavailable,
                async (response, uri) => new CustomWebException(
                    await GetWebExceptionMessageAsync(response).ConfigureAwait(false),
                    uri));
            mappings.Add(
                HttpStatusCode.RequestTimeout,
                async (response, uri) => new CustomWebException(
                    await GetWebExceptionMessageAsync(response).ConfigureAwait(false),
                    uri));
            mappings.Add(
                HttpStatusCode.BadGateway,
                async (response, uri) => new CustomWebException(
                    await GetWebExceptionMessageAsync(response).ConfigureAwait(false),
                    uri));
            mappings.Add(
                HttpStatusCode.GatewayTimeout,
                async (response, uri) => new CustomWebException(
                    await GetWebExceptionMessageAsync(response).ConfigureAwait(false),
                    uri));
            return mappings;
        }

        /// <summary>
        /// Gets the custom error mappings.
        /// </summary>
        /// <returns>The <see cref="IReadOnlyDictionary"/>.</returns>
        private IReadOnlyDictionary<HttpStatusCode, Func<HttpResponseMessage, Uri, Task<WebException>>> GetCustomErrorMappings()
        {
            if (this.customHttpStatusCodes == null || !this.customHttpStatusCodes.Any())
            {
                return GetDefaultErrorMappings();
            }
            else
            {
                var mappings = new Dictionary<HttpStatusCode, Func<HttpResponseMessage, Uri, Task<WebException>>>();
                foreach (var httpStatusCode in this.customHttpStatusCodes)
                {
                    mappings.Add(
                        httpStatusCode,
                        async (response, uri) => new CustomWebException(
                            await GetWebExceptionMessageAsync(response).ConfigureAwait(false),
                            uri,
                            response.StatusCode));
                }

                return mappings;
            }
        }
    }
}
