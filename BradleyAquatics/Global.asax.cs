using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BradleyAquatics.Models;

namespace BradleyAquatics
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected void Application_AuthenticateRequest()
        //{

        //    // Check if user is logged in
        //    if (User == null) { return; }

        //    // Get username
        //    string username = Context.User.Identity.Name;

        //    // Populate roles
        //    using (BradleyAquaticsDbEntities46 dbtwo = new BradleyAquaticsDbEntities46())
        //    {
        //        tblUser user = dbtwo.tblUser.FirstOrDefault(x => x.Username == "admin");
        //    }
        //    // Build IPrincipal object
        //    IIdentity userIdentity = new GenericIdentity(username);
        //    IPrincipal newUserObj = new GenericPrincipal(userIdentity);

        //    // Update Context.User
        //    Context.User = newUserObj;
        //}

    }
}
