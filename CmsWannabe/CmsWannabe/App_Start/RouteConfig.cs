using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CmsWannabe
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "CreatePage",
                url: "admin/createpage",
                defaults: new { controller = "Admin", action = "CreatePage"}
            );

            routes.MapRoute(
                name: "RenderTemplates",
                url: "{param1}/{param2}/{param3}",
                defaults: new { controller = "Admin", action = "RenderTemplate",
                    param1 = UrlParameter.Optional,
                    param2 = UrlParameter.Optional,
                    param3 = UrlParameter.Optional }
                );
        }
    }
}
