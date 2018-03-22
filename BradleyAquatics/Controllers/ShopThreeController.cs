using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using BradleyAquatics.Models;
using BradleyAquatics.Areas.Admin.Controllers;
using System.IO;


namespace BradleyAquatics.Controllers
{
    public class ShopThreeController : ShopController
    {
        //public BradleyAquaticsDbEntities111 dbtwo = new BradleyAquaticsDbEntities111();

        // GET: ShopTwo
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }


        public ActionResult CategoryMenuPartial()
        {
            //var catlist = from i in dbtwo.tblCategories
            //              select i;

            //catlist = catlist.OrderBy(i => i.Sorting);

            // Declare and initiate list

            List<tblCategory> catlist = dbtwo.tblCategories.ToArray().OrderBy(x => x.Sorting).ToList();

            return PartialView(catlist);

            //Return partial with list
        }

        //GET: ShopTwo/category/name

        //public ActionResult Category(string name)
        //{
        //    //var catlist = from i in dbtwo.tblCategories
        //    //              select i;

        //    //catlist = catlist.Where(x => x.Slug == name);
        //    var cat = dbtwo.tblCategories.Where(x => x.Slug == name).FirstOrDefault();

        //    //var cat = catlist.Where(x => x.Slug == name).FirstOrDefault();

        //    int catId = cat.Id;



        //    //Init the list
        //    var listOfProducts = dbtwo.tblproducts.ToArray()
        //                            .Where(x => x.CategoryId == catId)
        //                            .Select(x => x)
        //                            .ToList();

        //    //Get Category name

        //    var productCat = dbtwo.tblproducts.Where(x => x.CategoryId == catId).FirstOrDefault();
        //    ViewBag.CategoryName = productCat.CategoryName;

        //    //Return view with the list 
        //    return View(listOfProducts);
        //}

        public ActionResult All()

        {
            //Declare a list of ProductVM
            List<tblproduct> productList;

            productList = dbtwo.tblproducts.ToArray().ToList();

            //Return view with list
            return View(productList);

        }




        // GET: /shopTwo/category/name

        public ActionResult Category(string name)

        {
            //Declare a list of ProductVM
            List<tblproduct> productList;


            //Get category id

            //int catId = dbtwo.tblCategories.Where(x => x.Slug == name).First().Id;
            tblCategory categoryDTO = dbtwo.tblCategories.Where(x => x.Slug == name).FirstOrDefault();
            int catId = categoryDTO.Id;

            //Init the list
            productList = dbtwo.tblproducts.ToArray().Where(x => x.CategoryId == catId).Select(x => x).ToList();

            //Get category name
            var productCat = dbtwo.tblproducts.Where(x => x.CategoryId == catId).FirstOrDefault();
            ViewBag.CategoryName = productCat.CategoryName;


            //Return view with list
            return View(productList);
        }

        // GET: /shop/product-details/name
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {

            //init product Id

            int id = 0;

            //Check if product exists

            if (!dbtwo.tblproducts.Any(x => x.Slug.Equals(name)))
            {

                return RedirectToAction("Index", "ShopTwo");
            }



            //init model

            tblproduct model = new tblproduct();

            model = dbtwo.tblproducts.Where(x => x.Slug == name).FirstOrDefault();

            id = model.Id;

            // get gallery images 

            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                     .Select(fn => Path.GetFileName(fn));

            return View("ProductDetails", model);
        }


        
    }
}