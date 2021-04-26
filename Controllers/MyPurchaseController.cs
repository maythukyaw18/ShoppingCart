using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartCA.Common;
using ShoppingCartCA.Container;
using ShoppingCartCA.Models;

namespace ShoppingCartCA.Controllers
{
    public class MyPurchaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [SessionCheck]
        public IActionResult PurchaseHistory()
        {
            SessionData session = HttpContext.Session.GetComplexData<SessionData>("CAShopping_User");
            SessionCartData cart = HttpContext.Session.GetComplexData<SessionCartData>("CAShopping_Cart");
            ViewData["SessionData"] = session;
            ViewData["cartItemCount"] = (session != null ? CartFunctions.GetCartItemCount(session.UserName) : (cart == null ? 0 : cart.getCartItemCount()));

            List<MyPurchase> myPurchases = new List<MyPurchase>();
            myPurchases = MyPurchaseData.GetPurchaseHeaderByUserName(session.UserName);
            List<List<PurchaseDetails>> purchaseDetails = new List<List<PurchaseDetails>>();
            foreach (var p in myPurchases)
            {
                p.Review = ReviewFunction.GetPurchasedProduceReview(p.PurchaseID, p.ProductID);
                purchaseDetails.Add(MyPurchaseData.GetActivationCodes(p.ProductID, p.PurchaseID));

            }
            ViewData["myPurchase"] = myPurchases;
            ViewData["activationCode"] = purchaseDetails;
            
            

            if(myPurchases.Count() == 0)
            {
                ViewData["norecord"] = "No record found.";
            }
            
            return View();
        }

        public IActionResult AddReview(int productId, int purchaseId, int ratingScore)
        {
            SessionData session = HttpContext.Session.GetComplexData<SessionData>("CAShopping_User");

            ReviewFunction.AddProductReview(productId, purchaseId, ratingScore);
            return RedirectToAction("PurchaseHistory", "MyPurchase");
        }
        

    }
}