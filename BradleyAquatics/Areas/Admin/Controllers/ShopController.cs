using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BradleyAquatics.Areas.Admin.Models.ViewModels.ShopThree;
using BradleyAquatics.Models;
using BradleyAquatics.Models.ViewModels.Pages;
using BradleyAquatics.Models.ViewModels.Shop;
using PagedList;

namespace BradleyAquatics.Areas.Admin.Controllers
{
    //[Authorize(Users = "admin")]
    public class ShopController : Controller
    {
        public BradleyAquaticsDbEntities100 dbtwo = new BradleyAquaticsDbEntities100();



        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            var catlist = from i in dbtwo.tblCategories
                          select i;

            catlist = catlist.OrderBy(i => i.Sorting);


            return View(catlist);

        }

        //POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName, tblCategory catlist)
        {

            //Declare id
            string id;

            //check that the category is unique

            if (dbtwo.tblCategories.Any(x => x.Name == catName))
                return "titletaken";

            catlist.Name = catName;
            catlist.Slug = catName.Replace(" ", "-").ToLower();
            catlist.Sorting = 100;

            dbtwo.tblCategories.Add(catlist);
            dbtwo.SaveChanges();

            //Get the id

            id = catlist.Id.ToString();
            //return id

            return id;

        }

        //POST: Admin/Shop/ReorderCategories

        [HttpPost]
        public void ReorderCategories(int[] id)
        {

            //set intital count
            int count = 1;

            // set sorting for each category

            foreach (var catId in id)
            {
                tblCategory catlist = dbtwo.tblCategories.Find(catId);

                // arranges item - by sorting value given in drag and drop function
                catlist.Sorting = count;

                dbtwo.SaveChanges();
                count++;
            }
        }

        //Get:Admin/Shop/DeleteCategory
        public ActionResult DeleteCategory(int id)
        {
            tblCategory catlist = dbtwo.tblCategories.Find(id);
            dbtwo.tblCategories.Remove(catlist);
            dbtwo.SaveChanges();
            return RedirectToAction("Categories");
        }

        //POST:Admin/Shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            //check category name is unique
            if (dbtwo.tblCategories.Any(x => x.Name == newCatName))
                return "titletaken";

            tblCategory catlist = dbtwo.tblCategories.Find(id);

            catlist.Name = newCatName;
            catlist.Slug = newCatName.Replace(" ", "-").ToLower();

            //save changes

            dbtwo.SaveChanges();
            return "ok";
        }



        //GET:Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {

            tblproduct model = new tblproduct
            {
                Categories = new SelectList(dbtwo.tblCategories.ToList(), "Id", "Name")
            };

            return View(model);

        }
        //POST:Admin/Shop/AddProduct
        [HttpPost]
        public ActionResult AddProduct([Bind(Include = "Id,Name,Slug,Price,Description,CategoryName,CategoryId,ImageName")]tblproduct model, HttpPostedFileBase file)
        {
            //check model state 

            if (!ModelState.IsValid)
            {
                model.Categories = new SelectList(dbtwo.tblCategories.ToList(), "Id", "Name");
                return View(model);
            }

            //make sure product name is unique

            if (dbtwo.tblproducts.Any(x => x.Name == model.Name))
            {
                model.Categories = new SelectList(dbtwo.tblCategories.ToList(), "Id", "Name");
                ModelState.AddModelError("", "That product name is taken!");
                return View(model);
            }

            //Declare product id
            int id;


            model.Slug = model.Name.Replace(" ", "-").ToLower();

            var product = dbtwo.tblCategories.FirstOrDefault(x => x.Id == model.CategoryId);

            model.CategoryName = product.Name;

            dbtwo.tblproducts.Add(model);
            dbtwo.SaveChanges();

            //Get the Id
            id = model.Id;

            //Set TempData message
            TempData["SM"] = "You have added a product!";

            #region upload Image

            //create necesary directories to store images

            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            //check if a file was uploaded

            if (file != null && file.ContentLength > 0)
            {
                //Get file extension 
                string ext = file.ContentType.ToLower();

                //Verify extension 
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {

                    model.Categories = new SelectList(dbtwo.tblCategories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                    return View(model);

                }

                //Init image name 

                string imageName = file.FileName;

                //Save image name 

                tblproduct prolist = dbtwo.tblproducts.Find(id);
                prolist.ImageName = imageName;

                dbtwo.SaveChanges();

                //Set original and thumb image paths

                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);


                //save original 
                file.SaveAs(path);

                //Create and save thumb

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);


                #endregion Image


            }

            //redirect
            return RedirectToAction("AddProduct");
        }


        //GET: Admin/Shop/Products

        public ActionResult Products(int? page, int? catId)
        {


            //Set page Number

            var pageNumber = page ?? 1;


            //init the list 

            //List<tblproduct> listOfProducts;

            var listOfProducts = dbtwo.tblproducts.ToArray()
                                .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                                .Select(x => x)
                                .ToList();





            ////populate categories select list 

            ViewBag.Categories = new SelectList(dbtwo.tblCategories.ToList(), "Id", "Name");

            ////set selected Category 

            ViewBag.SelectedCat = catId.ToString();

            ////set Pagination 

            var onePageOfProducts = listOfProducts.ToPagedList(pageNumber, 3);
            ViewBag.onePageOfProducts = onePageOfProducts;

            //Return view with list 
            return View(listOfProducts);
        }

        //GET: Admin/Shop/EditProduct
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            // Get the product
            tblproduct productlist = dbtwo.tblproducts.Find(id);
            //

            //Confirm page exists
            if (productlist == null)
            {
                return Content("The product does not exist");
            }

            //init model

            tblproduct model = productlist;


            // make a select list
            model.Categories = new SelectList(dbtwo.tblCategories.ToList(), "Id", "Name");


            //Get all gallery files
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                      .Select(fn => Path.GetFileName(fn));


            //return view with model,
            return View(productlist);
        }

        //POST: Admin/Shop/EditProduct
        [HttpPost]
        public ActionResult EditProduct([Bind(Include = "Id,Name,Slug,Description,Price,CategoryName,CategoryId,ImageName")] tblproduct model, HttpPostedFileBase file)
        {
            // Get the product

            int id = model.Id;

            

            //Populate categories select list and gallery images
            model.Categories = new SelectList(dbtwo.tblCategories.ToList(), "Id", "Name");
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                      .Select(fn => Path.GetFileName(fn));


            //check model state

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Make sure product name is unique

            if (dbtwo.tblproducts.Where(x => x.Id != id).Any(x => x.Name == model.Name))
            {
                ModelState.AddModelError("", "That product name is taken!");
                return View(model);
            }

            //Update Product 

            model.Slug = model.Name.Replace(" ", "-").ToLower();

            var product = dbtwo.tblCategories.FirstOrDefault(x => x.Id == model.CategoryId);

            model.CategoryName = product.Name;
           

            dbtwo.Entry(model).State = EntityState.Modified;
            dbtwo.SaveChanges();

            //Set TempData message
            TempData["SM"] = "You have edited the product!";

            #region Image upload
            // Check for file upload
            if (file != null && file.ContentLength > 0)
            {

                // Get extension
                string ext = file.ContentType.ToLower();

                // Verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                    return View(model);
                }

                // Set uplpad directory paths
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");

                // Delete files from directories

                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file2 in di1.GetFiles())
                    file2.Delete();

                foreach (FileInfo file3 in di2.GetFiles())
                    file3.Delete();

                // Save image name

                string imageName = file.FileName;

                tblproduct prolist = dbtwo.tblproducts.Find(id);
                prolist.ImageName = imageName;
                dbtwo.SaveChanges();


                // Save original and thumb images

                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);

                #endregion
            }


                //Redirect
                return RedirectToAction("EditProduct");
        }


        //Get:Admin/Shop/DeleteProduct
        public ActionResult DeleteProduct(int id)
        {

            //delete producte from database
            tblproduct productlist = dbtwo.tblproducts.Find(id);
            dbtwo.tblproducts.Remove(productlist);
            dbtwo.SaveChanges();

            //Delete product folder

            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            string pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());

            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);


            //Redirect
        
            return RedirectToAction("Products");
        }

        // POST: Admin/Shop/SaveGalleryImages
        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            // Loop through files
            foreach (string fileName in Request.Files)
            {
                // Init the file
                HttpPostedFileBase file = Request.Files[fileName];

                // Check it's not null
                if (file != null && file.ContentLength > 0)
                {
                    // Set directory paths
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                    string pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

                    // Set image paths
                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);

                    // Save original and thumb

                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }

            }

        }

        // POST: Admin/Shop/DeleteImage
        [HttpPost]
        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);
        }


        // GET: Admin/Shop/Orders
        public ActionResult Orders()
        {
            // Init list of OrdersForAdminVM
            List<OrdersForAdminVM> ordersForAdmin = new List<OrdersForAdminVM>();

            
                // Init list of OrderVM
                List<OrderVM> orders = dbtwo.tblOrders.ToArray().Select(x => new OrderVM(x)).ToList();

                // Loop through list of OrderVM
                foreach (var order in orders)
                {
                    // Init product dict
                    Dictionary<string, int?> productsAndQty = new Dictionary<string, int?>();

                    // Declare total
                    decimal? total = 0m;

                    // Init list of OrderDetailsDTO
                    List<tblOrderDetail> orderDetailsList = dbtwo.tblOrderDetails.Where(X => X.OrderId == order.OrderId).ToList();

                    // Get username
                    tblUser user = dbtwo.tblUsers.Where(x => x.Id == order.UserId).FirstOrDefault();
                    string username = user.Username;

                    // Loop through list of OrderDetailsDTO
                    foreach (var orderDetails in orderDetailsList)
                    {
                        // Get product
                        tblproduct product = dbtwo.tblproducts.Where(x => x.Id == orderDetails.ProductId).FirstOrDefault();

                        // Get product price
                        decimal? price = product.Price;

                        // Get product name
                        string productName = product.Name;

                    int? quantity = orderDetails.Quantity;

                   

                    // Add to product dict



                    productsAndQty.Add(productName, orderDetails.Quantity);

                        // Get total
                        total += orderDetails.Quantity * price;
                    }

                    // Add to ordersForAdminVM list
                    ordersForAdmin.Add(new OrdersForAdminVM()
                    {
                        OrderNumber = order.OrderId,
                        Username = username,
                        Total = total,
                        ProductsAndQty = productsAndQty,
                        CreatedAt = order.CreatedAt
                    });
                }
            

            // Return view with OrdersForAdminVM list
            return View(ordersForAdmin);
        }

    }
}