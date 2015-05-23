using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace klientwebowy
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{par1}/{par2}/{par3}/{par4}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    par1 = UrlParameter.Optional,
                    par2 = UrlParameter.Optional,
                    par3 = UrlParameter.Optional,
                    par4 = UrlParameter.Optional
                }
            );
        }
    }
}
