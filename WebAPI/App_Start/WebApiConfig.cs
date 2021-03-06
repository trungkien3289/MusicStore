﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using WebAPI.ActionFilters;
using WebApiThrottle;

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

            config.Formatters.JsonFormatter.SupportedMediaTypes
            .Add(new MediaTypeHeaderValue("text/html"));
            //config.Routes.MapHttpRoute(
            //   name: "ActionBased",
            //   routeTemplate: "api/{controller}/action/{action}/{id}",
            //   defaults: new { id = RouteParameter.Optional }
            //);

            config.MessageHandlers.Add(new ThrottlingHandler()
            {
                Policy = new ThrottlePolicy(perSecond: 10, perMinute: 40, perHour: 300, perDay: 300, perWeek: 6000)
                {
                    IpThrottling = true
                },
                Repository = new CacheRepository()
            });
        }
    }
}
