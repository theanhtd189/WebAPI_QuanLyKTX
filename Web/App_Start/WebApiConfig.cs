using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = RouteParameter.Optional }
            );

/*            config.Routes.MapHttpRoute(
                name: "API_1",
                routeTemplate: "api/{controller}/{action}/page={page}&limit={limit}",
                defaults: new { controller = "Default", action = "Get", page=1, limit=10 }
            );*/

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
            = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}
