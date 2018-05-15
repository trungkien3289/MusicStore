using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebAPI.ActionFilters;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "ActionBased",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new LoggingFilterAttribute());
            //config.Routes.MapHttpRoute(
            //   name: "ActionBased",
            //   routeTemplate: "api/{controller}/action/{action}/{id}",
            //   defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
