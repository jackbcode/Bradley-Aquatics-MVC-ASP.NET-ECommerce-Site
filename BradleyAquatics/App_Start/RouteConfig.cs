using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BradleyAquatics
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Account", "Account/{action}/{id}", new { controller = "Account", action = "Index", id = UrlParameter.Optional }, new[] { "BradleyAquatics.Controllers" });

            routes.MapRoute("Cart", "Cart/{action}/{id}", new { controller = "Cart", action = "Index", id = UrlParameter.Optional }, new[] { "BradleyAquatics.Controllers" });

            routes.MapRoute("ShopThree", "ShopThree/{action}/{name}", new { controller = "ShopThree", action = "Index", name = UrlParameter.Optional }, new[] { "BradleyAquatics.Controllers" });

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });

           

          

            //routes.MapRoute("PagesMenuPartial", "Pages/PagesMenuPartial", new { controller = "Pages", action = "PagesMenuPartial" }, new[] { "BradleyAquatics.Controllers" });
            
            //routes.MapRoute("SidebarPartial", "Pages/SidebarPartial", new { controller = "Pages", action = "SidebarPartial" }, new[] { "CmsShoppingCart.Controllers" });
            //routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" }, new[] { "BradleyAquatics.Controllers" });
           



        }    
    }
}
