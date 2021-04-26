
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartCA.Common;
using ShoppingCartCA.Container;
using ShoppingCartCA.Models;

namespace ShoppingCartCA.Controllers
{
    public class CartController : Controller
    {
        //static List<Cart> carts = new List<Cart>();

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ViewCart(string userName, string sessionId)
        {
            SessionData session = HttpContext.Session.GetComplexData<SessionData>("CAShopping_User");
            SessionCartData cart = HttpContext.Session.GetComplexData<SessionCartData>("CAShopping_Cart");

            List<Cart> CartItems = null;

            if (session != null)
            {
                CartItems = CartFunctions.GetCartItems(session.UserName);
            }
            else
            {
                if (cart != null)
                {
                    CartItems = cart.CartItem;
                    foreach (var cartItem in CartItems)
                    {
                        cartItem.Product = ProductData.GetProduct(cartItem.ProductID);
                    }
                }
            }

            ViewData["SessionData"] = session;
            ViewData["cartItemCount"] = (session != null ? CartFunctions.GetCartItemCount(session.UserName) : (cart == null ? 0 : cart.getCartItemCount()));
            ViewData["CartItems"] = CartItems;
            return View();
        }
         
        [SessionCheck]
        public ActionResult CheckOut()
        {
            SessionData session = HttpContext.Session.GetComplexData<SessionData>("CAShopping_User");
            SessionCartData cart = HttpContext.Session.GetComplexData<SessionCartData>("CAShopping_Cart");
            
            int purchaseId = CartFunctions.AddPurchaseHeader(session.UserName);

            List<Cart> items = CartFunctions.GetCartItems(session.UserName);
            foreach (Cart item in items)
            {
                while (item.Quantity > 0)
                {
                    string activationCode = Guid.NewGuid().ToString();
                    //!! Unique Check Required !!
                    CartFunctions.AddPurchaseDetails(purchaseId, item.ProductID, activationCode);
                    item.Quantity--;
                }
            }
            CartFunctions.EmptyCart(session.UserName);

            return RedirectToAction("PurchaseHistory", "MyPurchase");
        }

        public ActionResult RemoveFromCart(int productId)
        {
            SessionData session = HttpContext.Session.GetComplexData<SessionData>("CAShopping_User");
            SessionCartData cart = HttpContext.Session.GetComplexData<SessionCartData>("CAShopping_Cart");

            if (session != null)
            {
                CartFunctions.RemoveFromCart(productId, session.UserName);
            }
            else
            {
                cart.CartItem.RemoveAt(cart.findCartItemIndex(productId));
                HttpContext.Session.SetComplexData("CAShopping_Cart", cart);
            }

            return RedirectToAction("ViewCart", "Cart");
        }

        public IActionResult UpdateQuantity(int productId, int qty)
        {
            SessionData session = HttpContext.Session.GetComplexData<SessionData>("CAShopping_User");
            SessionCartData cart = HttpContext.Session.GetComplexData<SessionCartData>("CAShopping_Cart");

            if (session != null)
            {
                CartFunctions.UpdateQuantity(productId, session.UserName, qty);
            }
            else
            {
                cart.CartItem[cart.findCartItemIndex(productId)].Quantity = qty;
                HttpContext.Session.SetComplexData("CAShopping_Cart", cart);
            }
           
            return RedirectToAction("ViewCart", "Cart");
        }


    }
}