using System;
using Owin;
using Microsoft.Owin.Security.OAuth;

namespace Web.Api.Auth.TokenBased
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId { get; private set; }
        public void ConfigureAuth(IAppBuilder app)
        {
            
        }
    }
}