using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Owin;

namespace CredentialBasedTokenAuthDemo.Api
{
    public static class WebApiConfig
    {
        public static HttpConfiguration Register()
        {
            HttpConfiguration config = new HttpConfiguration(); 
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.EnableCors();
            return config;
        }
    }
}
