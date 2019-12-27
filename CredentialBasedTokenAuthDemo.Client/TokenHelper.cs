using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CredentialBasedTokenAuthDemo.Client
{
    public class TokenHelper
    {
        HttpClientWrapper httpClientWrapper;

        public TokenHelper(HttpClientWrapper httpClientWrapper)
        {
            this.httpClientWrapper = httpClientWrapper;
        }

        public Token GetToken(Dictionary<string, string> requestBody, string requestPath)
        {
            Token token = new Token();
            var tokenResponse = httpClientWrapper.PostAsync(requestBody, requestPath).Result;
            if (tokenResponse.IsSuccessStatusCode)
            {
                var JsonContent = tokenResponse.Content.ReadAsStringAsync().Result;
                token = JsonConvert.DeserializeObject<Token>(JsonContent);
                token.Error = null;
            }
            else
            {
                token.Error = "GetAccessToken failed likely due to an invalid client ID, client secret, or invalid usrename and password";
            }
            return token;
        }
    }
}
