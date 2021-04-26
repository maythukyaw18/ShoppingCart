using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Models
{
    public class MyPurchase
    {
        public int PurchaseID { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ProductImg { get; set; }
        public int Rating { get; set; }
        public Review Review { get; set; }



    }
}
