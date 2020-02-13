namespace Common.Wrapper.HttpClient
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Polly;
    using Polly.Retry;

    /// <summary>
    /// The HTTP client wrapper.
    /// </summary>
    public sealed class HttpClientWrapper : IHttpClientWrapper
    {
        #region Fields

        /// <summary>
        /// The class name.
        /// </summary>
        private const string ClassName = nameof(HttpClientWrapper);

        /// <summary>
        /// The API Key name.
        /// </summary>
        private const string ApiKey = "X-ApiKey";

        /// <summary>
        /// The authorization key.
        /// </summary>
        private const string AuthorizationKey = "Authorization";

        /// <summary>
        /// The min expiry time for token in minutes.
        /// </summary>
        private const double MinExpiryTimeForTokenInMinutes = 30;

        /// <summary>
        /// The retry policy.
        /// </summary>
        private readonly RetryPolicy retryPolicy;

        /// <summary>
        /// The exception handling helper
        /// </summary>
        private readonly IExceptionHandlingHelper exceptionHandlingHelper;

        /// <summary>
        /// The authentication key.
        /// </summary>
        private readonly string authenticationKey;

        /// <summary>
        /// The http client.
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// The is disposed.
        /// </summary>
        private bool isDisposed;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper"/> class.
        /// </summary>
        /// <param name="authenticationKey">
        /// The authentication Key.
        /// </param>
        public HttpClientWrapper(string authenticationKey)
        {
            this.httpClient = HttpClientFactory.Create();
            this.httpClient.DefaultRequestHeaders.ExpectContinue = false;
            this.retryPolicy = GetDefaultRetryPolicy();
            this.authenticationKey = authenticationKey;
            this.exceptionHandlingHelper = new ExceptionHandlingHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper"/> class.
        /// </summary>
        /// <param name="authenticationKey">
        /// The authentication key.
        /// </param>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        public HttpClientWrapper(string authenticationKey, TimeSpan timeout)
        {
            this.httpClient = HttpClientFactory.Create();
            this.httpClient.Timeout = timeout;
            this.httpClient.DefaultRequestHeaders.ExpectContinue = false;
            this.retryPolicy = GetDefaultRetryPolicy();
            this.authenticationKey = authenticationKey;
            this.exceptionHandlingHelper = new ExceptionHandlingHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper"/> class.
        /// </summary>
        /// <param name="baseAddress">
        /// The base address.
        /// </param>
        /// <param name="defaultHeaders">
        /// The default headers.
        /// </param>
        /// <param name="authenticationKey">
        /// The authentication Key.
        /// </param>
        public HttpClientWrapper(Uri baseAddress, IDictionary<string, string> defaultHeaders, string authenticationKey)
        {
            this.httpClient = HttpClientFactory.Create();
            this.httpClient.BaseAddress = baseAddress;
            this.httpClient.DefaultRequestHeaders.ExpectContinue = false;
            AddDefaultHeaders(this.httpClient, defaultHeaders);
            this.retryPolicy = GetDefaultRetryPolicy();
            this.authenticationKey = authenticationKey;
            this.exceptionHandlingHelper = new ExceptionHandlingHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="defaultHeaders">The default headers.</param>
        /// <param name="authenticationKey">The authentication key.</param>
        /// <param name="retryPolicy">The retry policy.</param>
        /// <param name="customHttpStatusCodes">The custom http status codes for which mappings are created.</param>
        public HttpClientWrapper(Uri baseAddress, IDictionary<string, string> defaultHeaders, string authenticationKey, RetryPolicy retryPolicy, List<HttpStatusCode> customHttpStatusCodes)
        {
            this.httpClient = HttpClientFactory.Create();
            this.httpClient.BaseAddress = baseAddress;
            this.httpClient.DefaultRequestHeaders.ExpectContinue = false;
            AddDefaultHeaders(this.httpClient, defaultHeaders);
            this.retryPolicy = retryPolicy ?? GetDefaultRetryPolicy();
            this.authenticationKey = authenticationKey;
            this.exceptionHandlingHelper = new ExceptionHandlingHelper(customHttpStatusCodes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="defaultHeaders">The default headers.</param>
        /// <param name="authenticationKey">The authentication key.</param>
        /// <param name="timeout">The timeout.</param>
        public HttpClientWrapper(Uri baseAddress, IDictionary<string, string> defaultHeaders, string authenticationKey, TimeSpan timeout)
        {
            this.httpClient = HttpClientFactory.Create();
            this.httpClient.Timeout = timeout;
            this.httpClient.BaseAddress = baseAddress;
            this.httpClient.DefaultRequestHeaders.ExpectContinue = false;
            AddDefaultHeaders(this.httpClient, defaultHeaders);
            this.retryPolicy = GetDefaultRetryPolicy();
            this.authenticationKey = authenticationKey;
            this.exceptionHandlingHelper = new ExceptionHandlingHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper"/> class.
        /// </summary>
        /// <param name="defaultHeaders">
        /// The default headers.
        /// </param>
        /// <param name="authenticationKey">
        /// The authentication Key.
        /// </param>
        public HttpClientWrapper(IDictionary<string, string> defaultHeaders, string authenticationKey)
        {
            this.httpClient = HttpClientFactory.Create();
            this.httpClient.DefaultRequestHeaders.ExpectContinue = false;
            AddDefaultHeaders(this.httpClient, defaultHeaders);
            this.retryPolicy = GetDefaultRetryPolicy();
            this.authenticationKey = authenticationKey;
            this.exceptionHandlingHelper = new ExceptionHandlingHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper"/> class.
        /// </summary>
        /// <param name="defaultHeaders">The default headers.</param>
        /// <param name="authenticationKey">The authentication key.</param>
        /// <param name="timeout">The timeout.</param>
        public HttpClientWrapper(IDictionary<string, string> defaultHeaders, string authenticationKey, TimeSpan timeout)
        {
            this.httpClient = HttpClientFactory.Create();
            this.httpClient.Timeout = timeout;
            this.httpClient.DefaultRequestHeaders.ExpectContinue = false;
            AddDefaultHeaders(this.httpClient, defaultHeaders);
            this.retryPolicy = GetDefaultRetryPolicy();
            this.authenticationKey = authenticationKey;
            this.exceptionHandlingHelper = new ExceptionHandlingHelper();
        }

        #endregion

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            if (!this.isDisposed && this.httpClient != null)
            {
                this.httpClient.Dispose();
                this.httpClient = null;
            }

            this.isDisposed = true;
        }

        #region Interface Methods

        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> GetAsync(
            Uri requestUri,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders)
        {
            return await this.SendAsyncWithRetryAsync(
                       HttpMethod.Get,
                       requestUri,
                       customHeaders,
                       null,
                       requiredAlternateHeaders,
                       null).ConfigureAwait(false);
        }

        /// <summary>
        /// The get asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="customHeaders">
        /// The custom header.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <param name="claims">
        /// The claims.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> GetAsync(
            Uri requestUri,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claims)
        {
            return await this.SendAsyncWithRetryAsync(
                       HttpMethod.Get,
                       requestUri,
                       customHeaders,
                       null,
                       requiredAlternateHeaders,
                       claims).ConfigureAwait(false);
        }

        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await this.SendAsyncWithRetryAsync(HttpMethod.Get, requestUri, null, null, null, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>
        ///  /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return await this.DeleteAsync(HttpMethod.Delete, requestUri, null, null, null, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="customHeaders">The custom headers.</param>
        /// <param name="requiredAlternateHeaders">The required alternate headers.</param>
        /// <param name="claims">The claims.</param>
        /// <returns>
        ///  /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> DeleteAsync(
            Uri requestUri,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claims)
        {
            return await this.DeleteAsync(
                       HttpMethod.Delete,
                       requestUri,
                       customHeaders,
                       null,
                       requiredAlternateHeaders,
                       claims).ConfigureAwait(false);
        }

        /// <summary>
        /// The post async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> PostAsync(Uri requestUri, JObject entity)
        {
            var result = await this.SendAsyncWithRetryAsync(HttpMethod.Post, requestUri, null, entity, null, null)
                                             .ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// The post async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> PostAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders)
        {
            var result = await this.SendAsyncWithRetryAsync(
                                             HttpMethod.Post,
                                             requestUri,
                                             customHeaders,
                                             entity,
                                             requiredAlternateHeaders,
                                             null).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// The post asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <param name="claims">
        /// The claims.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> PostAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claims)
        {
            var result = await this.SendAsyncWithRetryAsync(
                                             HttpMethod.Post,
                                             requestUri,
                                             customHeaders,
                                             entity,
                                             requiredAlternateHeaders,
                                             claims).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// The post without response content async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task PostWithoutResponseContentAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders)
        {
            await this.SendAsyncWithRetryAndNoResponseContentAsync(
                HttpMethod.Post,
                requestUri,
                customHeaders,
                entity,
                requiredAlternateHeaders,
                null).ConfigureAwait(false);
        }

        /// <summary>
        /// The post without response content asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <param name="claims">
        /// The claims.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task PostWithoutResponseContentAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claims)
        {
            await this.SendAsyncWithRetryAndNoResponseContentAsync(
                HttpMethod.Post,
                requestUri,
                customHeaders,
                entity,
                requiredAlternateHeaders,
                claims).ConfigureAwait(false);
        }

        /// <summary>
        /// The post without response content async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task PostWithoutResponseContentAsync(Uri requestUri, JObject entity)
        {
            await this.SendAsyncWithRetryAndNoResponseContentAsync(HttpMethod.Post, requestUri, null, entity, null, null)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// The put async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> PutAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders)
        {
            return await this.SendAsyncWithRetryAsync(
                       HttpMethod.Put,
                       requestUri,
                       customHeaders,
                       entity,
                       requiredAlternateHeaders,
                       null).ConfigureAwait(false);
        }

        /// <summary>
        /// The put asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <param name="claims">
        /// The claims.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> PutAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claims)
        {
            return await this.SendAsyncWithRetryAsync(
                       HttpMethod.Put,
                       requestUri,
                       customHeaders,
                       entity,
                       requiredAlternateHeaders,
                       claims).ConfigureAwait(false);
        }

        /// <summary>
        /// The put async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<HttpResponseMessage> PutAsync(Uri requestUri, JObject entity)
        {
            return await this.SendAsyncWithRetryAsync(HttpMethod.Put, requestUri, null, entity, null, null).ConfigureAwait(false);
        }

        /// <summary>
        /// The put without response content async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task PutWithoutResponseContentAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders)
        {
            await this.SendAsyncWithRetryAndNoResponseContentAsync(
                HttpMethod.Put,
                requestUri,
                customHeaders,
                entity,
                requiredAlternateHeaders,
                null).ConfigureAwait(false);
        }

        /// <summary>
        /// The put without response content asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <param name="claims">
        /// The claims.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task PutWithoutResponseContentAsync(
           Uri requestUri,
           JObject entity,
           IDictionary<string, string> customHeaders,
           IDictionary<string, string> requiredAlternateHeaders,
           IDictionary<string, string> claims)
        {
            await this.SendAsyncWithRetryAndNoResponseContentAsync(
                HttpMethod.Put,
                requestUri,
                customHeaders,
                entity,
                requiredAlternateHeaders,
                claims).ConfigureAwait(false);
        }

        /// <summary>
        /// The put without response content async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task PutWithoutResponseContentAsync(Uri requestUri, JObject entity)
        {
            await this.SendAsyncWithRetryAndNoResponseContentAsync(HttpMethod.Put, requestUri, null, entity, null, null)
                .ConfigureAwait(false);
        }

        #endregion

        #region Private

        /// <summary>
        /// The add body to message if exists.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <typeparam name="TBody">The generic type
        /// </typeparam>
        private static void AddBodyToMessageIfExists<TBody>(HttpRequestMessage request, TBody body)
            where TBody : class
        {
            if (body != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    Encoding.UTF8,
                    "application/json");
            }
        }

        /// <summary>
        /// The add default headers.
        /// </summary>
        /// <param name="httpClient">
        /// The http client.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        private static void AddDefaultHeaders(HttpClient httpClient, IDictionary<string, string> customHeaders)
        {
            if (customHeaders != null)
            {
                foreach (KeyValuePair<string, string> header in customHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// The add headers to message.
        /// </summary>
        /// <param name="requestMessage">
        /// The request message.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        private static void AddHeadersToMessage(
            HttpRequestMessage requestMessage,
            IDictionary<string, string> customHeaders)
        {
            if (customHeaders != null)
            {
                foreach (KeyValuePair<string, string> header in customHeaders)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// The get default retry policy.
        /// </summary>
        /// <returns>
        /// The <see cref="RetryPolicy"/>.
        /// </returns>
        private static RetryPolicy GetDefaultRetryPolicy()
        {
            var policy = Policy.Handle<CustomWebException>().WaitAndRetryAsync(
                new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3) },
                (exception, timeSpan) => { });
            return policy;
        }

        /// <summary>
        /// The read response message async.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <typeparam name="T">The generic type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private static async Task<T> ReadResponseMessageAsync<T>(HttpResponseMessage message)
        {
            if (typeof(T) == typeof(HttpResponseMessage))
            {
                return (T)(object)message;
            }

            var entity = await message.Content.ReadAsAsync<T>().ConfigureAwait(false);
            return entity;
        }

        /// <summary>
        /// The replace header dictionary values.
        /// </summary>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="alternateHeaders">
        /// The alternate headers.
        /// </param>
        private static void ReplaceHeaderDictionaryValues(
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> alternateHeaders)
        {
            if (customHeaders != null && alternateHeaders != null)
            {
                foreach (var key in alternateHeaders.Keys)
                {
                    customHeaders[key] = alternateHeaders[key];
                }
            }
        }

        /// <summary>
        /// The create exception from error message async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="responseMsg">
        /// The response message.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<WebException> CreateExceptionFromErrorMessageAsync(
            Uri requestUri,
            HttpResponseMessage responseMsg)
        {
            Func<HttpResponseMessage, Uri, Task<WebException>> errorResponseHandler;
            if (this.exceptionHandlingHelper.GetErrorMappings().TryGetValue(responseMsg.StatusCode, out errorResponseHandler))
            {
                return await errorResponseHandler(responseMsg, requestUri).ConfigureAwait(false);
            }

            return new WebException(
                $"Failed to execute successfully with status code {responseMsg.StatusCode}. No mapped exception was found to provide a more detailed error.");
        }

        /// <summary>
        /// The get response.
        /// </summary>
        /// <param name="httpMethod">
        /// The HTTP method.
        /// </param>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">The Invalid Operation Exception
        /// </exception>
        /// <exception cref="CustomWebException">The Custom Web Exception
        /// </exception>
        private async Task<HttpResponseMessage> GetResponseAsync(
            HttpMethod httpMethod,
            Uri requestUri,
            IDictionary<string, string> customHeaders,
            JObject body)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(httpMethod, requestUri))
            {
                AddHeadersToMessage(request, customHeaders);
                AddBodyToMessageIfExists(request, body);

                try
                {
                    var responseMsg = await this.httpClient.SendAsync(request).ConfigureAwait(false);

                    if (responseMsg == null)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                "The response message was null when executing operation {0}.",
                                request.Method));
                    }

                    return responseMsg;
                }
                catch (WebException ex)
                {
                    var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    if (statusCode == HttpStatusCode.InternalServerError || statusCode == HttpStatusCode.Conflict
                        || statusCode == HttpStatusCode.ServiceUnavailable
                        || statusCode == HttpStatusCode.RequestTimeout)
                    {
                        throw new CustomWebException(ex.Message, ex, requestUri);
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the response for delete.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="customHeaders">The custom headers.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">The Invalid Operation Exception
        /// </exception>
        /// <exception cref="CustomWebException">The Custom Web Exception
        /// </exception>
        private async Task<HttpResponseMessage> GetResponseForDeleteAsync(
          HttpMethod httpMethod,
          Uri requestUri,
          IDictionary<string, string> customHeaders)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(httpMethod, requestUri))
            {
                AddHeadersToMessage(request, customHeaders);

                try
                {
                    var responseMsg = await this.httpClient.DeleteAsync(requestUri).ConfigureAwait(false);

                    if (responseMsg == null)
                    {
                        throw new InvalidOperationException(
                            $"The response message was null when executing operation {request.Method}.");
                    }

                    return responseMsg;
                }
                catch (WebException ex)
                {
                    var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    if (statusCode == HttpStatusCode.InternalServerError || statusCode == HttpStatusCode.Conflict
                        || statusCode == HttpStatusCode.ServiceUnavailable
                        || statusCode == HttpStatusCode.RequestTimeout)
                    {
                        throw new CustomWebException(ex.Message, ex, requestUri);
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// The send asynchronous.
        /// </summary>
        /// <param name="httpMethod">
        /// The HTTP method.
        /// </param>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <param name="claimsDictionary">The claims dictionary.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The Object Disposed Exception
        /// </exception>
        private async Task<HttpResponseMessage> SendAsync(
            HttpMethod httpMethod,
            Uri requestUri,
            IDictionary<string, string> customHeaders,
            JObject body,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claimsDictionary)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(ClassName);
            }

            var apiKey = string.Empty;
            var result = customHeaders != null && customHeaders.TryGetValue(ApiKey, out apiKey);
            if (result)
            {
                if (claimsDictionary == null)
                {
                    claimsDictionary = new Dictionary<string, string> { { ApiKey, apiKey } };
                }
                else
                {
                    claimsDictionary[ApiKey] = apiKey;
                }

                var token = AuthorizationHelper.CreateJwtToken(this.authenticationKey, claimsDictionary, MinExpiryTimeForTokenInMinutes, requestUri.ToString());
                customHeaders[AuthorizationKey] = token;
            }

            var httpResponseMessage = await this.GetResponseAsync(httpMethod, requestUri, customHeaders, body)
                                          .ConfigureAwait(false);
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized
                || httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                ReplaceHeaderDictionaryValues(customHeaders, requiredAlternateHeaders);
                if (result)
                {
                    claimsDictionary[ApiKey] = customHeaders[ApiKey];
                    var token = AuthorizationHelper.CreateJwtToken(this.authenticationKey, claimsDictionary, MinExpiryTimeForTokenInMinutes, requestUri.ToString());
                    customHeaders[AuthorizationKey] = token;
                }

                httpResponseMessage = await this.GetResponseAsync(httpMethod, requestUri, customHeaders, body)
                                          .ConfigureAwait(false);
            }

            return httpResponseMessage;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="customHeaders">The custom headers.</param>
        /// <param name="body">The body.</param>
        /// <param name="requiredAlternateHeaders">The required alternate headers.</param>
        /// <param name="claimsDictionary">The claims dictionary.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The Object Disposed Exception
        /// </exception>
        private async Task<HttpResponseMessage> DeleteAsync(
            HttpMethod httpMethod,
            Uri requestUri,
            IDictionary<string, string> customHeaders,
            JObject body,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claimsDictionary)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(ClassName);
            }

            var apiKey = string.Empty;
            var result = customHeaders != null && customHeaders.TryGetValue(ApiKey, out apiKey);
            if (result)
            {
                if (claimsDictionary == null)
                {
                    claimsDictionary = new Dictionary<string, string> { { ApiKey, apiKey } };
                }
                else
                {
                    claimsDictionary[ApiKey] = apiKey;
                }

                var token = AuthorizationHelper.CreateJwtToken(this.authenticationKey, claimsDictionary, MinExpiryTimeForTokenInMinutes, requestUri.ToString());
                customHeaders[AuthorizationKey] = token;
            }

            var httpResponseMessage = await this.GetResponseForDeleteAsync(httpMethod, requestUri, customHeaders)
                                          .ConfigureAwait(false);
            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized
                || httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                ReplaceHeaderDictionaryValues(customHeaders, requiredAlternateHeaders);
                if (result)
                {
                    claimsDictionary[ApiKey] = customHeaders[ApiKey];
                    var token = AuthorizationHelper.CreateJwtToken(this.authenticationKey, claimsDictionary, MinExpiryTimeForTokenInMinutes, requestUri.ToString());
                    customHeaders[AuthorizationKey] = token;
                }

                httpResponseMessage = await this.GetResponseAsync(httpMethod, requestUri, customHeaders, body)
                                          .ConfigureAwait(false);
            }

            return httpResponseMessage;
        }

        /// <summary>
        /// The send asynchronous with retry.
        /// </summary>
        /// <param name="httpMethod">
        /// The http method.
        /// </param>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <param name="claimsDictionary">
        /// The claims Dictionary.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="WebException">
        /// The web exception.
        /// </exception>
        private async Task<HttpResponseMessage> SendAsyncWithRetryAsync(
            HttpMethod httpMethod,
            Uri requestUri,
            IDictionary<string, string> customHeaders,
            JObject body,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claimsDictionary)
        {
            var statusCode = HttpStatusCode.OK;
            var isStatusChanged = false;
            HttpContent responseContent = null;
            var policyResult = await this.retryPolicy.ExecuteAndCaptureAsync(
                                                                 async () =>
                                                                 {
                                                                     var responseMessage =
                                                                         await this.SendAsync(
                                                                                 httpMethod,
                                                                                 requestUri,
                                                                                 customHeaders,
                                                                                 body,
                                                                                 requiredAlternateHeaders,
                                                                                 claimsDictionary)
                                                                             .ConfigureAwait(false);

                                                                     if (responseMessage.IsSuccessStatusCode)
                                                                     {
                                                                         return await ReadResponseMessageAsync<HttpResponseMessage>(
                                                                                    responseMessage).ConfigureAwait(
                                                                                    false);
                                                                     }

                                                                     statusCode = responseMessage.StatusCode;
                                                                     isStatusChanged = true;
                                                                     responseContent = responseMessage.Content;
                                                                     throw await this
                                                                               .CreateExceptionFromErrorMessageAsync(
                                                                                   new Uri(
                                                                                       this.httpClient.BaseAddress,
                                                                                       requestUri),
                                                                                   responseMessage).ConfigureAwait(
                                                                                   false);
                                                                 }).ConfigureAwait(false);

            if (policyResult.Outcome != OutcomeType.Successful)
            {
                statusCode = isStatusChanged ? statusCode : HttpStatusCode.InternalServerError;
                var httpResponseMessage = new HttpResponseMessage(statusCode);
                httpResponseMessage.Content = responseContent;
                return httpResponseMessage;
            }

            return policyResult.Result;
        }

        /// <summary>
        /// The send asynchronous with retry and no response content.
        /// </summary>
        /// <param name="httpMethod">
        /// The HTTP method.
        /// </param>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="customHeaders">
        /// The custom headers.
        /// </param>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <param name="requiredAlternateHeaders">
        /// The required alternate headers.
        /// </param>
        /// <param name="claimsDictionary">The claims dictionary</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="WebException">The web exception.
        /// </exception>
        /// <exception cref="Exception">The exception.
        /// </exception>
        private async Task SendAsyncWithRetryAndNoResponseContentAsync(
            HttpMethod httpMethod,
            Uri requestUri,
            IDictionary<string, string> customHeaders,
            JObject body,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claimsDictionary)
        {
            var policyResult = await this.retryPolicy.ExecuteAndCaptureAsync(
                                            async () =>
                                            {
                                                var responseMessage =
                                                    await this.SendAsync(
                                                        httpMethod,
                                                        requestUri,
                                                        customHeaders,
                                                        body,
                                                        requiredAlternateHeaders,
                                                        claimsDictionary).ConfigureAwait(false);
                                                if (responseMessage.IsSuccessStatusCode)
                                                {
                                                    return;
                                                }

                                                throw await this.CreateExceptionFromErrorMessageAsync(
                                                          new Uri(this.httpClient.BaseAddress, requestUri),
                                                          responseMessage).ConfigureAwait(false);
                                            }).ConfigureAwait(false);

            if (policyResult.Outcome != OutcomeType.Successful)
            {
                throw policyResult.FinalException;
            }
        }

        #endregion
    }
}
