using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class PurchaseDetails
    {
        public int PurchaseID { get; set; }
        public int ProductID { get; set; }
        public string ActivationCode { get; set; }
    }
}
