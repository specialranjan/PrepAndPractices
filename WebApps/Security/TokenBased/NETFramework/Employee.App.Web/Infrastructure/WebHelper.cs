namespace Employee.App.Web.Infrastructure
{
    using System.Configuration;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Employee.App.Common;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    public class WebHelper
    {
        public async Task<string> GetTokens()
        {
            string authority = ConfigurationHelper.AADInstance + ConfigurationHelper.TenantId;
            AuthenticationContext authContext = new AuthenticationContext(authority);
            ClientCredential clientCredential = new ClientCredential(ConfigurationHelper.ClientId, ConfigurationHelper.ClientSecret);
            string userObjectId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;            
            AuthenticationResult result;
            try
            {
                result = await authContext.AcquireTokenSilentAsync(ConfigurationHelper.GraphResourceId, clientCredential, new UserIdentifier(userObjectId, UserIdentifierType.UniqueId)).ConfigureAwait(false);
            }
            catch (AdalSilentTokenAcquisitionException)
            {
                var token = (await authContext.AcquireTokenAsync(ConfigurationHelper.GraphResourceId, clientCredential).ConfigureAwait(false)).AccessToken;
                return token;
            }

            return result.AccessToken;
        }
    }
}