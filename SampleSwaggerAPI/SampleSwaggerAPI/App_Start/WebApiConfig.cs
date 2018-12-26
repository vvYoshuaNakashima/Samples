using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using WebApiContrib.Formatting.Jsonp;
using Newtonsoft.Json.Serialization;

namespace SampleSwaggerAPI
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// WEB API 設定
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API の設定およびサービス

            //// JSONP 対応
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.AddJsonpFormatter();

            // Web API ルート
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DetailApi",
            //    routeTemplate: "api/{controller}/{action}/{id}/params/{parameter}"
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
