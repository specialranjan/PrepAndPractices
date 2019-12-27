using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;


[assembly: OwinStartup(typeof(CredentialBasedTokenAuthDemo.Api.Startup))]
namespace CredentialBasedTokenAuthDemo.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // token generation
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                // for demo purposes
                AllowInsecureHttp = true,

                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),

                Provider = new CredentialBasedTokenAuthDemo.Api.Infrastructure.AuthorizationServerProviders.OAuth()
            });

            // token consumption
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            app.UseWebApi(WebApiConfig.Register());
        }
    }
}