namespace Employee.App.Api
{
    using System;
    using Owin;
    using Microsoft.Owin.Security.ActiveDirectory;
    using System.Globalization;
    using Microsoft.IdentityModel.Tokens;

    public partial class Startup
	{
		public void ConfigureAuth(IAppBuilder app)
		{
            //string validAudience = "https://microsoft.onmicrosoft.com/7c54ed50-8bba-42d9-ad6b-1150d4384d11";
            //var tenant = validAudience.Substring(0, validAudience.LastIndexOf('/'));

            //         app.UseWindowsAzureActiveDirectoryBearerAuthentication(
            //             new WindowsAzureActiveDirectoryBearerAuthenticationOptions
            //             {
            //                 Tenant = tenant.ToLower(CultureInfo.InvariantCulture).Replace("https://", ""),
            //                 TokenValidationParameters = new TokenValidationParameters
            //                 {
            //                     SaveSigninToken = true,
            //                     ValidAudience = validAudience.ToLower()
            //                 },
            //             });

            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Tenant = "72f988bf-86f1-41af-91ab-2d7cd011db47",
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = "7c54ed50-8bba-42d9-ad6b-1150d4384d11",
                        SaveSigninToken = true
                    }
                });
        }
	}
}