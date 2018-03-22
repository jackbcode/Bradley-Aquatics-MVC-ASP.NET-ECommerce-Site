using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BradleyAquatics.Models;
using BradleyAquatics.Models.ViewModels.Account;
using BradleyAquatics.Models.ViewModels;
using static BradleyAquatics.Models.ViewModels.Account.UserNavPartialVM;
using System.Collections.Generic;
using BradleyAquatics.Models.ViewModels.Shop;

namespace BradleyAquatics.Controllers
{


    public class AccountController : Controller
    {
        public BradleyAquaticsDbEntities100 dbtwo = new BradleyAquaticsDbEntities100();

        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        // GET: /account/login
        [HttpGet]
        public ActionResult Login()
        {
            // Confirm user is not logged in

            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
                return RedirectToAction("user-profile");

            // Return view
            return View();
        }


        // POST: /account/login
        [HttpPost]
        public ActionResult Login(LoginUserVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if the user is valid

            bool isValid = false;

            if (dbtwo.tblUsers.Any(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password)))
            {
                isValid = true;
            }


            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid Username or Password.");
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, model.RememberMe));
            }
        }


        // GET: /account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        // POST: /account/create-account
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount([Bind(Include = "Id,FirstName,LastName,EmailAddress,UserName,Password,ConfirmPassword")]  tblUser model)
        {



            // Check model state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", model);
            }


            //// Check if passwords match
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View("CreateAccount", model);
            }



            // Make sure username is unique
            if (dbtwo.tblUsers.Any(x => x.Username == model.Username))
            {
                ModelState.AddModelError("", "Username " + model.Username + " is taken.");
                model.Username = "";
                return View("CreateAccount", model);
            }


            dbtwo.tblUsers.Add(model);
            dbtwo.SaveChanges();

            // Create a TempData message
            TempData["SM"] = "You are now registered and can login.";

            // Redirect
            return Redirect("~/account/login");
        }


        // GET: /account/Logout
      
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["cart"] = null;
            return Redirect("~/account/login");
        }

   
        public ActionResult UserNavPartial()
        {
            // Get username
            string username = User.Identity.Name;

            // Declare model
            UserNavPartialVM model;

           
                // Get the user
                tblUser userinfo = dbtwo.tblUsers.FirstOrDefault(x => x.Username == username);

                // Build the model
                model = new UserNavPartialVM()
                {
                    FirstName = userinfo.FirstName,
                    LastName = userinfo.LastName
                };
            

            // Return partial view with model
            return PartialView(model);
        }

        // GET: /account/user-profile
        [HttpGet]
        [ActionName("user-profile")]
        public ActionResult UserProfile()
        {
            // Get username
            string username = User.Identity.Name;

            // Declare model
            UserProfileVM model;


            // Get user
            tblUser userinfo = dbtwo.tblUsers.FirstOrDefault(x => x.Username == username);
          

                // Build model
                model = new UserProfileVM(userinfo);
            

            // Return view with model
            return View("UserProfile", model);
        }

        // POST: /account/user-profile
        [HttpPost]
        [ActionName("user-profile")]
        public ActionResult UserProfile(UserProfileVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            // Check if passwords match if need be
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                    return View("UserProfile", model);
                }
            }

            
                // Get username
                string username = User.Identity.Name;
            

            // Make sure username is unique
            if (dbtwo.tblUsers.Where(x => x.Id != model.Id).Any(x => x.Username == username))
                {
                    ModelState.AddModelError("", "Username " + model.Username + " already exists.");
                    model.Username = "";
                    return View("UserProfile", model);
                }

                // Edit DTO
                tblUser userinfo= dbtwo.tblUsers.Find(model.Id);

               userinfo.FirstName = model.FirstName;
                userinfo.LastName = model.LastName;
               userinfo.EmailAddress = model.EmailAddress;
                userinfo.Username = model.Username;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    userinfo.Password = model.Password;
                userinfo.ConfirmPassword = model.ConfirmPassword;
                }

                // Save
                dbtwo.SaveChanges();
            

            // Set TempData message
            TempData["SM"] = "You have edited your profile!";

            // Redirect
            return Redirect("~/account/user-profile");
        }


        // GET: /account/Orders
        
        public ActionResult Orders()
        {
            // Init list of OrdersForUserVM
            List<ordersForUserVM> ordersForUser = new List<ordersForUserVM>();


            // Get user id
            tblUser user = dbtwo.tblUsers.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            int userId = user.Id;

                // Init list of OrderVM
                List<OrderVM> orders = dbtwo.tblOrders.Where(x => x.UserId == userId).ToArray().Select(x => new OrderVM(x)).ToList();

                // Loop through list of OrderVM
                foreach (var order in orders)
                {
                    // Init products dict
                    Dictionary<string, int?> productsAndQty = new Dictionary<string, int?>();

                    // Declare total
                    decimal? total = 0m;

                    // Init list of OrderDetails
                    List<tblOrderDetail> orderDetailsList = dbtwo.tblOrderDetails.Where(x => x.OrderId == order.OrderId).ToList();

                    // Loop though list of OrderDetails
                    foreach (var orderDetails in orderDetailsList)
                    {
                        // Get product
                        tblproduct product = dbtwo.tblproducts.Where(x => x.Id == orderDetails.ProductId).FirstOrDefault();

                        // Get product price
                        decimal? price = product.Price;

                        // Get product name
                        string productName = product.Name;

                        // Add to products dict
                        productsAndQty.Add(productName, orderDetails.Quantity);

                        // Get total
                        total += orderDetails.Quantity * price;
                    }

                    // Add to OrdersForUserVM list
                    ordersForUser.Add(new ordersForUserVM()
                    {
                        OrderNumber = order.OrderId,
                        Total = total,
                        ProductsAndQty = productsAndQty,
                        CreatedAt = order.CreatedAt
                    });
                }

            

            // Return view with list of OrdersForUserVM
            return View(ordersForUser);
        }



    }


}   