using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace JXCWPlus_Back
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //设置webapi的帮助页面信息
            WebApiTestClient.WebApiTestClientHttpMessageHandler
                .RegisterRouteForTestClient(GlobalConfiguration.Configuration);
            //跨域配置
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
