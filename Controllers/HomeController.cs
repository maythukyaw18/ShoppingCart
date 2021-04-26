using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCartCA.Common;
using ShoppingCartCA.Container;
using ShoppingCartCA.Models;

namespace ShoppingCartCA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [NoSessionCheck]
        public ActionResult Login(string UserName, string Password)
        {
            SessionCartData cart = HttpContext.Session.GetComplexData<SessionCartData>("CAShopping_Cart");
            ViewData["cartItemCount"] = (cart == null ? 0 : cart.getCartItemCount());

            if (UserName == null && Password == null)
                return View();
            using (MD5 md5Hash = MD5.Create())
            {
                Password = Md5Hash.GetMd5Hash(md5Hash, Password);
            }

            UserInfo User = UserData.GetUserByUsernameAndPassword(UserName, Password);

            if (User == null)
            {
                ViewData["error"] = "Invalid username or password, please retry.";
                return View();
            }
            else
            {
                SessionData session = new SessionData();
                session.UserName = UserName;
                session.FirstName = User.FirstName;
                session.LastName = User.LastName;
                session.SessionID = ShoppingCartCA.Models.Session.CreateSession(UserName);
                HttpContext.Session.SetComplexData("CAShopping_User", session);

                if(cart != null)
                {
                    for(int i=0; i < cart.CartItem.Count; i++)
                    {
                        CartFunctions.AddItemToCart(cart.CartItem[i].ProductID, cart.CartItem[i].Quantity, session.UserName, session.SessionID);
                    }
                    HttpContext.Session.SetComplexData("CAShopping_Cart", null);
                    return RedirectToAction("ViewCart", "Cart");
                }                
                return RedirectToAction("ViewProducts", "ProductGallery");

            }
        }
        

        public ActionResult Logout(string SessionId)
        {
            ShoppingCartCA.Models.Session.RemoveSession(SessionId);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
