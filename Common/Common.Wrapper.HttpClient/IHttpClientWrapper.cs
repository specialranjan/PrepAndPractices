namespace Common.Wrapper.HttpClient
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The HttpClientWrapper interface.
    /// </summary>
    public interface IHttpClientWrapper : IDisposable
    {
        /// <summary>
        /// The get asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
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
        Task<HttpResponseMessage> GetAsync(
            Uri requestUri,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders);

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
        Task<HttpResponseMessage> GetAsync(
            Uri requestUri,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claims);

        /// <summary>
        /// The delete async.
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
        /// <param name="claims">
        /// The claims.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpResponseMessage> DeleteAsync(
           Uri requestUri,
           IDictionary<string, string> customHeaders,
           IDictionary<string, string> requiredAlternateHeaders,
           IDictionary<string, string> claims);

        /// <summary>
        /// The get asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpResponseMessage> GetAsync(Uri requestUri);

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri);

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
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpResponseMessage> PostAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders);

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
        Task<HttpResponseMessage> PostAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claims);

        /// <summary>
        /// The post asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpResponseMessage> PostAsync(Uri requestUri, JObject entity);

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
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task PostWithoutResponseContentAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders);

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
        Task PostWithoutResponseContentAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claims);

        /// <summary>
        /// The post without response content asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task PostWithoutResponseContentAsync(Uri requestUri, JObject entity);

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
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpResponseMessage> PutAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders);

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
        Task<HttpResponseMessage> PutAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claims);

        /// <summary>
        /// The put asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<HttpResponseMessage> PutAsync(Uri requestUri, JObject entity);

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
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task PutWithoutResponseContentAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders);

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
        Task PutWithoutResponseContentAsync(
            Uri requestUri,
            JObject entity,
            IDictionary<string, string> customHeaders,
            IDictionary<string, string> requiredAlternateHeaders,
            IDictionary<string, string> claims);

        /// <summary>
        /// The put without response content asynchronous.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task PutWithoutResponseContentAsync(Uri requestUri, JObject entity);
    }
}
