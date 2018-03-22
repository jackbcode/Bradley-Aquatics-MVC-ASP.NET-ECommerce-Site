using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BradleyAquatics.Models;
using BradleyAquatics.Models.ViewModels.Cart;

namespace BradleyAquatics.Controllers
{
    public class CartController : Controller
    {
        public BradleyAquaticsDbEntities100 dbtwo = new BradleyAquaticsDbEntities100();

        // GET: Cart
        public ActionResult Index()
        {
            // Init the cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            // Check if cart is empty
            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty";
                return View();
            }

            // Calculate total and save to ViewBag

            decimal? total = 0m;

            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.GrandTotal = total;

            // Return view with list
            return View(cart);
            
        }

        public ActionResult CartPartial()
        {
            // Init CartVM
            CartVM model = new CartVM();

            // Init quantity
            int qty = 0;

            // Init price
            decimal? price = 0m;

            // Check for cart session
            if (Session["cart"] != null)
            {
                // Get total qty and price
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;
                }

                model.Quantity = qty;
                model.Price = price;

            }
            else
            {
                // Or set qty and price to 0
                model.Quantity = 0;
                model.Price = 0m;
            }

            // Return partial view with model
            return PartialView(model);
        }

        public ActionResult AddToCartPartial(int id)
        {
            // Init CartVM list
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            // Init CartVM
            CartVM model = new CartVM();
           
            {
                // Get the product
         
                tblproduct product = dbtwo.tblproducts.Find(id);
                    

                // Check if the product is already in cart
                var productInCart = cart.FirstOrDefault(x => x.ProductId == id);

                // If not, add new
                if (productInCart == null)
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = 1,
                        Price = product.Price,
                        Image = product.ImageName
                    });
                }
                else
                {
                    // If it is, increment
                    productInCart.Quantity++;
                }
            }

            // Get total qty and price and add to model

            int qty = 0;
            decimal? price = 0m;   //have to change????

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }

            model.Quantity = qty;
            model.Price = price;

            // Save cart back to session
            Session["cart"] = cart;

            // Return partial view with model
            return PartialView(model);
        }

        //Get: /Cart/IncrementProduct
        public JsonResult IncrementProduct(int productId)
        {
            //init cart list

            List<CartVM> cart = Session["cart"] as List<CartVM>;

            //Get cartVM from list 

            CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

            //Increment qty

            model.Quantity++;

            //store needed data.

            var result = new { qty = model.Quantity, price = model.Price };

            // Return json with data

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Get: /Cart/DecrementProduct
        public JsonResult DecrementProduct(int productId)
        {
            //init cart list

            List<CartVM> cart = Session["cart"] as List<CartVM>;

            //Get cartVM from list 

            CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

            //Decrement qty
            if(model.Quantity > 1)
            {
                model.Quantity--;
            }
            else
            {
                model.Quantity = 0;
                cart.Remove(model);
            }

            //store needed data.

            var result = new { qty = model.Quantity, price = model.Price };

            // Return json with data
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Get: /Cart/RemoveProduct

        public void RemoveProduct(int productId)
        {
            //init cart list

            List<CartVM> cart = Session["cart"] as List<CartVM>;

            //Get cartVM from list 

            CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

            //Remove model from list
            cart.Remove(model);
        }

        public ActionResult PaypalPartial()
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;
            return PartialView(cart);
        }

        //Post: /Cart/PlaceOrder
        [HttpPost]
        public void PlaceOrder()
        {
            // Get cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            // Get username
            string username = User.Identity.Name;

            int orderId = 0;


            // Init tblorder table
            tblOrder neworder = new tblOrder();

            // Get user id

            
            var q = dbtwo.tblUsers.FirstOrDefault(x => x.Username == username);

          

            int userId = q.Id;

            // Add to Order and save
       
          
           neworder.UserId = userId;
            
          

            
            neworder.CreatedAt = DateTime.Now;

            dbtwo.tblOrders.Add(neworder);

            dbtwo.SaveChanges();

            // Get inserted id
             orderId = neworder.OrderId;

            // Init OrderDetailsDTO
            tblOrderDetail newod = new tblOrderDetail();

            // Add to OrderDetailsDTO
            foreach (var item in cart)
            {
                newod.OrderId = orderId;
                newod.UserId = userId;
                newod.ProductId = item.ProductId;
                newod.Quantity = item.Quantity;

                dbtwo.tblOrderDetails.Add(newod);

                dbtwo.SaveChanges();
            }


            //Email admin 

            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("f930400e40024a", "6b28fe7a9576ea"),
                EnableSsl = true
            };
            client.Send("admin@example.com", "admin@example.com", "New Order", "You have a new order. Order number " + orderId);

            //Reset session
            Session["cart"] = null;


        }
    }
}