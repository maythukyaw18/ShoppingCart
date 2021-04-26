using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class Review
    {
        public int ProductID { get; set; }
        public int PurchaseID { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
