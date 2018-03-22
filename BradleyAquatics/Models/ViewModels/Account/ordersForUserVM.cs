using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BradleyAquatics.Models.ViewModels.Account
{
    public class ordersForUserVM
    {
        public int OrderNumber { get; set; }
        public decimal? Total { get; set; }
        public Dictionary<string, int?> ProductsAndQty { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}