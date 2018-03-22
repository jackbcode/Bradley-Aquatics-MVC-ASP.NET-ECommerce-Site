using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BradleyAquatics.Models;
using BradleyAquatics.Models.ViewModels.Pages;

namespace BradleyAquatics.Areas.Admin.Controllers
{
    //[Authorize(Users = "admin")]
    public class PagesController : Controller
    {

      
        public BradleyAquaticsDbEntities100 db = new BradleyAquaticsDbEntities100();
        // GET: Admin/Pages
        public ActionResult Index()
        {

            var pagelist = from i in db.tblpages
                           select i;

            pagelist = pagelist.OrderBy(i => i.Sorting);


            return View(pagelist);
        }

        // GET: Admin/Pages/AddPage

        public ActionResult AddPage()
        {
            return View();
        }


        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage([Bind(Include = "Id,Title,Slug,Body,Sorting,HasSideBar")] tblpage pagelist)

        {
            if (!ModelState.IsValid)
            {
                return View(pagelist);
            }



            //Check for and set slug if need be 
            if (pagelist.Slug == null)
            {

                pagelist.Slug = pagelist.Title.Replace(" ", "-").ToLower();
            }
            else
            {
                pagelist.Slug = pagelist.Slug.Replace(" ", "-").ToLower();
            }

            //Make sure title and slug are unique 

            if (db.tblpages.Any(x => x.Title == pagelist.Title) || db.tblpages.Any(x => x.Slug == pagelist.Slug))
            {
                ModelState.AddModelError("", " That title or slug already exists");
                return View(pagelist);
            }

            pagelist.Sorting = 100;

            db.tblpages.Add(pagelist);
            db.SaveChanges();


            //Set TempData message
            TempData["SM"] = " You have added a new page!";

            //Redirect
            return RedirectToAction("AddPage");
        }


        // GET: Main/Editpage

        public ActionResult EditPage(int id)
        {
            tblpage pagelist = db.tblpages.Find(id);
            //

            //Confirm page exists
            if (pagelist == null)
            {
                return Content("The page does not exist");
            }
            return View(pagelist);
        }

        //POST edit changes
        [HttpPost]
        public ActionResult EditPage([Bind(Include = "Id,Title,Slug,Body,Sorting,HasSideBar")] tblpage pagelist)
        {


            if (!ModelState.IsValid)
            {
                return View(pagelist);
            }

            // check for slug and set if it is necessary
            if (pagelist.Slug != "home")
            {
                if (string.IsNullOrWhiteSpace(pagelist.Slug))
                {
                    pagelist.Slug = pagelist.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    pagelist.Slug = pagelist.Slug.Replace(" ", "-").ToLower();
                }
            }

            //Make sure Title and slug are unique

            int id = pagelist.Id;

            if (db.tblpages.Where(x => x.Id != id).Any(x => x.Title == pagelist.Title) ||
                    db.tblpages.Where(x => x.Id != id).Any(x => x.Slug == pagelist.Slug))
            {

                ModelState.AddModelError("", "That title or slug already exists.");
                return View(pagelist);

            }

            if (pagelist.Sorting == null)
            {
                pagelist.Sorting = 100;
            }


           

                db.Entry(pagelist).State = EntityState.Modified;
                db.SaveChanges();

          

            TempData["SM"] = "You have edited the page!";
            return RedirectToAction("EditPage");

        }



        // GET: Main/Details/5
        public ActionResult PageDetails(int? id)
        {
            //get the page
            tblpage pagelist = db.tblpages.Find(id);

            //confirm page exists

            if (pagelist == null)
            {
                return Content("This page does not exist");
            }

            //return view with model

            return View(pagelist);



        }



        




        //// GET: Main/Delete/5
        //public ActionResult DeletePage(int? id)
        //{
            
        //    tblpage pagelist = db.tblpages.Find(id);
        //    if (pagelist == null)
        //    {
        //        return Content("This page does not exist");
        //    }
        //    return View(pagelist);
        //}

        // POST: Main/Delete/5
        //[HttpPost, ActionName("DeletePage")]
       
        public ActionResult Deletepage(int id)
        {
            tblpage pagelist = db.tblpages.Find(id);
            db.tblpages.Remove(pagelist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //POST: Admin/Pages/Reorderpages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
           
            //set intital count
            int count = 1;

            // set sorting for each page

            foreach (var pageId in id)
            {
                tblpage pagelist = db.tblpages.Find(pageId);

                // arranges item - by sorting value given in drag and drop function
                pagelist.Sorting = count;



                db.SaveChanges();
                count++;
            }
        }

       // //GET: Admin/Pages/EditSidebar
       // [HttpGet]
       // public ActionResult EditSidebar()
       // {
           
       //     tblSidebar model = db.tblSidebars.Find(1);
       //     return View(model);

       // }


       // //POST: Admin/Pages/EditSidebar
       //[HttpPost]
       // public ActionResult EditSidebar([Bind(Include = "Id,Body")] tblSidebar model)
       // {


       //     db.Entry(model).State = EntityState.Modified;
       //     db.SaveChanges();

       //     TempData["SM"] = "You have edited the sidebar!";

       //     return RedirectToAction("EditSidebar");


       // }


       // protected override void Dispose(bool disposing)
       // {
       //     if (disposing)
       //     {
       //         db.Dispose();
       //     }
       //     base.Dispose(disposing);
       // }
    }
}


     

    
