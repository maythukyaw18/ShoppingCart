using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class Purchase
    {
        public string PurchaseID { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
}
