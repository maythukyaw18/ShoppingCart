using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class Cart
    {
        public string UserName { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string ProductImg { get; set; }
        public Product Product { get; set; }
    }
}
