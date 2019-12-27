using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GetToken
{
    class Program
    {
        static void Main(string[] args)
        {

            var p = new Program();
            p.GetTokenAA().Wait();
        }

        private async Task GetTokenAA()
        {


            try
            {
                //string _authority = string.Format("https://login.microsoftonline.com/{0}", "bdoaaddt.onmicrosoft.com");
                string _authority = string.Format("https://login.microsoftonline.com/{0}", "ranjant.onmicrosoft.com");
                AuthenticationContext _authContext = new AuthenticationContext(_authority, false);
                
                var authResult = await _authContext.AcquireTokenAsync("https://ranjant.onmicrosoft.com/nativeapp2",
                    new ClientCredential("9ca94f69-b1a2-4aec-ae5a-3f83b36f5081", "0pQtdreEUNDnDLH/PtUMTbaccF7X5y+JA09wQqX5OUA="));


                //admToken = admAuth.GetAccessToken();
                // Create a header with the access_token property of the returned token
                var bearerToken = "Bearer " + authResult.AccessToken;
                Console.WriteLine(bearerToken);
                Console.ReadKey();
            }
            catch (WebException e)
            {

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }
    }
}

