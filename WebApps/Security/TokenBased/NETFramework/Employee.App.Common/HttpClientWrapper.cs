namespace Employee.App.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Employee.App.Common.Helpers;

    public class HttpClientWrapper
    {
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, string accessToken)
        {            
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var responseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);

                    if (responseMessage == null)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                "The response message was null when executing operation {0}.",
                                request.Method));
                    }

                    return responseMessage;
                }
                catch (WebException ex)
                {
                    var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    if (statusCode == HttpStatusCode.InternalServerError || statusCode == HttpStatusCode.Conflict
                        || statusCode == HttpStatusCode.ServiceUnavailable
                        || statusCode == HttpStatusCode.RequestTimeout)
                    {
                        throw;
                    }

                    throw;
                }
            }
        }
    }
}
