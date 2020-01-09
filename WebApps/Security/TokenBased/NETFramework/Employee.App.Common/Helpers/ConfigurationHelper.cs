namespace Employee.App.Common
{
    using System;
    using System.Configuration;

    public static class ConfigurationHelper
    {
        public static readonly string ClientId = GetValue("ida:clientId");
        public static readonly string ClientSecret = GetValue("ida:clientSecret");
        public static readonly string TenantId = GetValue("ida:tenantId");
        public static readonly string RedirectUri = GetValue("ida:redirectUri");
        public static readonly string AADInstance = GetValue("ida:AADInstance");
        public static readonly string PostLogoutRedirectUri = GetValue("ida:postLogoutRedirectUri");
        public static readonly string GraphResourceId = GetValue("graphResourceId");
        private static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
