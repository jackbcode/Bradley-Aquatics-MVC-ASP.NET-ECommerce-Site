using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradleyAquatics.Models.ViewModels.Cart
{
    public class CartVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }                          //might have to change
        public decimal? Total { get { return Quantity * Price; } }  //might have to change
        public string Image { get; set; }
    }
}