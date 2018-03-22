using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BradleyAquatics.Models;

namespace BradleyAquatics.Controllers
{
   
    public class PagesController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult About()
        {
           
            return View();
        }

        public ActionResult Contact()
        {
            

            return View();
        }





        //public BradleyAquaticsDbEntities111 db = new BradleyAquaticsDbEntities111();


        // GET: Index/{page}
        //public ActionResult Index(string page = "")
        //{


        //    // Get/set page slug
        //    if (page == "")
        //        page = "home";

        //    // Check if page exists

        //    if (!db.tblpages.Any(x => x.Slug.Equals(page)))
        //    {
        //        return RedirectToAction("Index", new { page = "" });
        //    }

        //    tblpage pagelist = db.tblpages.Where(x => x.Slug == page).FirstOrDefault();


        //    // Set page title
        //    ViewBag.PageTitle = pagelist.Title;

        //    return View(pagelist);
        //}

        //public ActionResult PagesMenuPartial()
        //{

        //  var pagelist = db.tblpages.ToArray()
        //                        .OrderBy(x => x.Sorting)
        //                        .Where(x => x.Slug != "home")
        //                        .Select(x => x);

        //    return PartialView(pagelist);
        //}

    }







}
