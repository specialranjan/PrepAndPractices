namespace Employee.App.Api
{
    using System.Web.Http;
    using System.Web.Http.Cors;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //var corsTenant = "https://localhost:44366/";
            //var cors = new EnableCorsAttribute(corsTenant, "*", "*");
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();            

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
