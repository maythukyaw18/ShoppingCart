using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartCA.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using ShoppingCartCA.Common;
using ShoppingCartCA.Container;
using Microsoft.AspNetCore.Razor.Language;

namespace ShoppingCartCA.Controllers
{
    public class ProductGalleryController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ViewProducts(string keyword, int AddToCartSuccess = 0, int AddedQty = 0, int AddedProductId = 0, int productId = 0)
        {
            SessionData session = HttpContext.Session.GetComplexData<SessionData>("CAShopping_User");
            SessionCartData cart = HttpContext.Session.GetComplexData<SessionCartData>("CAShopping_Cart");
            ViewData["SessionData"] = session;
            ViewData["cartItemCount"] = (session != null? CartFunctions.GetCartItemCount(session.UserName) : (cart == null ? 0 : cart.getCartItemCount()));
            List<Product> products = ProductData.GetProductDetails(keyword);

            foreach(var p in products)
            {
                p.ProductRatingAverage = ReviewFunction.GetProductReviewAverage(p.ProductID);
                p.ProductRatingCount = ReviewFunction.GetProductReviewCount(p.ProductID);
            }
            ViewData["products"] = products;

            //Success Message
            ViewData["AddToCartSuccess"] = AddToCartSuccess;
            ViewData["AddedQty"] = AddedQty;
            Product addedProductDetails = ProductData.GetProduct(AddedProductId);
            ViewData["AddedProductName"] = (addedProductDetails != null)? addedProductDetails.ProductName: null;

            return View();
        }
       
        public ActionResult AddToCart(int productID)
        {
            SessionData session = HttpContext.Session.GetComplexData<SessionData>("CAShopping_User");
            SessionCartData cart = HttpContext.Session.GetComplexData<SessionCartData>("CAShopping_Cart");
            ViewData["SessionData"] = session;

            int qty = 1;

            if (session != null && session.UserName != null)
            {
                //Login is done
                CartFunctions.AddItemToCart(productID, qty, session.UserName, session.SessionID);
            }
            else
            {
                if(cart == null)
                {
                    //If there is no product with the searched productId in the session's CartItem
                    Cart newItem = new Cart();

                    newItem.ProductID = productID;
                    newItem.Quantity = 1;

                    SessionCartData newCartSessionData = new SessionCartData();
                    newCartSessionData.CartItem = new List<Cart>();
                    newCartSessionData.CartItem.Add(newItem);

                    HttpContext.Session.SetComplexData("CAShopping_Cart", newCartSessionData);
                }
                else
                {
                    int productIndex = cart.findCartItemIndex(productID);

                    //result "-1" = means not found in CartItem
                    if(productIndex == -1)
                    {
                        //If there is no product with the searched productId in the session's CartItem
                        Cart newItem = new Cart();

                        newItem.ProductID = productID;
                        newItem.Quantity = qty;

                        cart.CartItem.Add(newItem);
                    }
                    else
                    { 
                        //If there is already a product with the productId in the session's CartItem
                        cart.CartItem[productIndex].Quantity += qty;
                    }

                    HttpContext.Session.SetComplexData("CAShopping_Cart", cart);
                }
            }

            return RedirectToAction("ViewProducts", new { AddToCartSuccess = 1, AddedProductId = productID, AddedQty = qty});
           
        }

        /*public static List<int[]> RetrieveCartItemsFromCookies()
        {
            var cookieName = "_ca_cart_items_cookie";

            List <int[]> list = new List<int[]>();
            HttpContext.Request.Cookies = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                string val = (!String.IsNullOrEmpty(keyName)) ? cookie[keyName] : cookie.Value;
                if (!String.IsNullOrEmpty(val)) return Uri.UnescapeDataString(val);
            }

            return list;

        } 

        //public static void StoreInCookie( string cookieName)
        /*
        public ActionResult RemovefromCart(int productID, string username)
        {
            
            CartFunctions.RemovefromCart(productId, userName);

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult EmptyCart(int productId, string userName)
        {
            
            CartFunctions.EmptyCart(userName);

            return Redirect(Request.UrlReferrer.ToString());
        }*/

    
    }
}