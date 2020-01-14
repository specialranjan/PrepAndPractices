namespace Dynamics.Crm.Oprations.Crud
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.WebServiceClient;
    using System.ServiceModel.Description;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    public sealed class OrgnizationService
    {
        private static IOrganizationService instance = null;
        private static readonly object instanceLock = new object();
        private readonly string clientId, clientSecret, authority, orgServiceUrl, orgServiceEndPoint;

        public OrgnizationService()
        {
            this.clientId = ConfigurationManager.AppSettings["ida:ClientId"];
            this.clientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
            this.authority = string.Concat(ConfigurationManager.AppSettings["ida:Authority"], ConfigurationManager.AppSettings["ida:TenantId"], "/");
            this.orgServiceUrl = ConfigurationManager.AppSettings["crm:OrgServiceUrl"];
            this.orgServiceEndPoint = ConfigurationManager.AppSettings["crm:OrgServiceEndpoint"];
        }

        public static IOrganizationService Instance
        {
            get 
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        OrgnizationService orgService = new OrgnizationService();
                        Uri endPoint = new Uri(string.Concat(orgService.orgServiceUrl, orgService.orgServiceEndPoint));
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        //string accessToken = orgService.GetAuthenticationToken().GetAwaiter().GetResult();
                        //instance = new OrganizationWebProxyClient(endPoint, false) { HeaderToken = accessToken };
                        ClientCredentials credential = new ClientCredentials();
                        credential.UserName.UserName = "ranjant@ranjant.onmicrosoft.com";
                        credential.UserName.Password = "JayRam@1985";
                        var proxy = new OrganizationServiceProxy(endPoint, null, credential, null);
                        proxy.EnableProxyTypes();
                        instance = (IOrganizationService)proxy;
                    }
                    return instance;
                }
            }
        }

        private async Task<string> GetAuthenticationToken()
        {
            //ClientCredential credentials = new ClientCredential(clientId, clientSecret);
            //var authContext = new AuthenticationContext(authority);
            //var result = await authContext.AcquireTokenAsync(orgServiceUrl, credentials).ConfigureAwait(false);
            AuthenticationContext authenticationContext = new AuthenticationContext("https://login.microsoftonline.com/d2fffb19-a311-407e-bf3f-798e69a8762f/oauth2/v2.0/authorize");
            ClientCredential credential = new ClientCredential("384245ed-16ee-4e41-b1a5-ca6ed997a558", "L.2f/?Cc47YdYa5CFnjS/aP9auH[WpJn");
            AuthenticationResult result = await authenticationContext.AcquireTokenAsync("https://ranjant.api.crm8.dynamics.com/", credential).ConfigureAwait(false);
            return result.AccessToken;
        }
    }
}
