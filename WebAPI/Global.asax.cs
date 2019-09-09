using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebAPI.Convertors;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //var settings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            //JsonSerializerSettings jSettings = new Newtonsoft.Json.JsonSerializerSettings()
            //{
            //    Formatting = Formatting.Indented,
            //    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            //};
            //jSettings.Converters.Add(new MyDateTimeConvertor());
            //settings = jSettings;

            JsonMediaTypeFormatter jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            JsonSerializerSettings jSettings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Culture = CultureInfo.GetCultureInfo("fr-FR")
            };
            //jSettings.Converters.Add(new MyDateTimeConvertor());
            jsonFormatter.SerializerSettings = jSettings;
        }
        protected void Application_BeginRequest()
        {
            //if (Request.Url.Scheme == "http")
            //{
            //    var path = "https://" + Request.Url.Host + Request.Url.PathAndQuery;
            //    Response.Status = "301 Moved Permanently";
            //    Response.AddHeader("Location", path);
            //}
        }
    }
}
