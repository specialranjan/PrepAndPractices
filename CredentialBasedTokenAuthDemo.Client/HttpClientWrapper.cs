
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CredentialBasedTokenAuthDemo.Client
{
    public class HttpClientWrapper
    {
        string userName, password, baseAddress, clientId, clientSecret;
        HttpClientHandler handler;
        HttpClient client;

        public HttpClientWrapper(string baseAddress, string clientId, string clientSecret, string userName, string password)
        {
            this.baseAddress = baseAddress;
            this.userName = userName;
            this.password = password;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.handler = new HttpClientHandler();
            this.client = new HttpClient(handler);
        }

        public async Task<HttpResponseMessage> PostAsync(Dictionary<string, string> requestBody, string requestPath)
        {
            string ClientIDandSecret = this.clientId + ":" + this.clientSecret;
            var authorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientIDandSecret));
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);
            return await client.PostAsync(this.baseAddress + requestPath, new FormUrlEncodedContent(requestBody)).ConfigureAwait(false);
        }

        public async Task<string> GetStringAsync(Token token, string requestPath)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return await client.GetStringAsync(this.baseAddress + requestPath).ConfigureAwait(false);
        }
    }
}
