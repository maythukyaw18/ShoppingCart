using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartCA.Common;
using ShoppingCartCA.Container;

namespace ShoppingCartCA.Controllers
{
    public class PaymentController : Controller
    {
        [SessionCheck]
        public IActionResult Index(string username, string sessionid)
        {
            SessionData session = HttpContext.Session.GetComplexData<SessionData>("CA_Shopping");
            //ViewData["UserName"] = HttpContext.Session.GetString("_UserName");
            //ViewData["SessionId"] = HttpContext.Session.GetString("_SessionId");
            return View();
        }

    }
}