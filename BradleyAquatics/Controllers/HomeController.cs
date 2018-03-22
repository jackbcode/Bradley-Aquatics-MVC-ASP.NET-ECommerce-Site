using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BradleyAquatics.Models;

namespace BradleyAquatics.Controllers
{
    public class HomeController : Controller
    {
        public BradleyAquaticsDbEntities100 dbtwo = new BradleyAquaticsDbEntities100();

        public ActionResult Index()
        {
            return View();


        }

       //GET - Search and display results

        public ActionResult SearchDetails(string searchString)
        {
           

            if (!dbtwo.tblproducts.Any(x => x.Slug.Contains(searchString)))
            {

                TempData["SM"] = "Sorry we do not have that fish";
                return RedirectToAction("Index", "Home");
            }

            return View(dbtwo.tblproducts.Where(x => x.Name.Contains(searchString)).ToList());

        }



        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {


            return View();
        }
    }
}