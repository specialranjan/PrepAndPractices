namespace Employee.App.Web
{
    using System;
	using System.IdentityModel.Claims;
    using System.Web;
    using Employee.App.Common;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Owin.Security;
	using Microsoft.Owin.Security.Cookies;
	using Microsoft.Owin.Security.OpenIdConnect;
	using Owin;	

	public partial class Startup
	{
		public void ConfigureAuth(IAppBuilder app)
		{
			app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
			app.UseCookieAuthentication(new CookieAuthenticationOptions());
			string authority = ConfigurationHelper.AADInstance + ConfigurationHelper.TenantId;
			string clientId = ConfigurationHelper.ClientId;
			string clientSecret = ConfigurationHelper.ClientSecret;
			OpenIdConnectAuthenticationNotifications notifications = new OpenIdConnectAuthenticationNotifications 
			{
				AuthorizationCodeReceived = async (context) =>
				{
					var code = context.Code;
					string signedInUserId = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
					AuthenticationContext authContext = new AuthenticationContext(authority);
					ClientCredential credential = new ClientCredential(clientId, clientSecret);
					AuthenticationResult result = await authContext.AcquireTokenByAuthorizationCodeAsync(code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential).ConfigureAwait(false);					
				}
			};

			app.UseOpenIdConnectAuthentication(
				new OpenIdConnectAuthenticationOptions
				{
					ClientId = clientId,
					Authority = authority,
					RedirectUri = ConfigurationHelper.RedirectUri,
					PostLogoutRedirectUri = ConfigurationHelper.PostLogoutRedirectUri,
					Scope = OpenIdConnectScope.OpenIdProfile,
					ResponseType = OpenIdConnectResponseType.IdToken,
					TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = false
					},					
					Notifications = notifications
				}
			);
		}
	}
}