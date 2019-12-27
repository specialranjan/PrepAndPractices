using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace CredentialBasedTokenAuthDemo.Client
{
    class Program
    {
        string baseAddress = "https://localhost:44365/", requestPath = "token";
        string clientId = "Client1", clientSecret = "Client1@Secret", userName = "TestUser1", password = "TestUser1@123";
        HttpClientWrapper httpClientWrapper;
        TokenHelper tokenHelper;

        public Program()
        {
            this.httpClientWrapper = new HttpClientWrapper(baseAddress, clientId, clientSecret, userName, password);
            tokenHelper = new TokenHelper(this.httpClientWrapper);
        }

        static void Main(string[] args)
        {
            var myObj = new Program();
            Console.WriteLine("Fetching Access Token from Authentication Server.");
            var token = myObj.GetToken();
            if (token.AccessToken != null)
            {
                Console.WriteLine("Fetching Access Token from Authentication Server succeeded.");
                Console.WriteLine("Calling Authentication Server to fetch identity principle.");
                myObj.CallService(token);

                token = myObj.RefreshToken(token);
                myObj.CallService(token);

                token = myObj.RefreshToken(token);
                myObj.CallService(token);
            }
            else
            {
                Console.WriteLine(token.Error);
            }

            Console.ReadLine();
        }

        private Token GetToken()
        {
            var requestBody = new Dictionary<string, string>
                {
                {"grant_type", "password"},
                {"username", userName},
                {"password", password},
            };

            return tokenHelper.GetToken(requestBody, this.requestPath);
        }

        private Token RefreshToken(Token token)
        {
            var requestBody = new Dictionary<string, string>
            {
                {"grant_type", "refresh_token"},
                {"refresh_token", token.RefreshToken}
            };

            return tokenHelper.GetToken(requestBody, this.requestPath);
        }

        private void CallService(Token token)
        {
            var response = httpClientWrapper.GetStringAsync(token, "api/identity").Result;
            Console.WriteLine(response);
        }
    }
}
