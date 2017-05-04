using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsWannabe.Extensions
{
    public class CustomViewEngine : RazorViewEngine
    {
        public CustomViewEngine()
        {
            //WidgetRegistry.Init();
            //var folders = WidgetRegistry.WidgetNames();

            var viewLocations = new List<string>{
            "~/Views/{1}/{0}.aspx",
            "~/Views/{1}/{0}.ascx",
            "~/Views/{1}/{0}.cshtml",
            "~/Views/Shared/{0}.aspx",
            "~/Views/Shared/{0}.ascx",
            "~/Views/Shared/{0}.cshtml",
            "~/AnotherPath/Views/{0}.ascx",
            "~/Views/Shared/Templates/{0}.cshtml"
            // etc
            };

            //foreach (var item in folders)
            //{
            //    viewLocations.Add("~/Views/" + item + "/{0}.cshtml");
            //}


            this.PartialViewLocationFormats = viewLocations.ToArray();
            this.ViewLocationFormats = viewLocations.ToArray();
        }
    }
}