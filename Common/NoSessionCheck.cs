using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using ShoppingCartCA.Common;
using ShoppingCartCA.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Controllers
{
    public class NoSessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessionData session = filterContext.HttpContext.Session.GetComplexData<SessionData>("CAShopping_User");
            if(session != null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "Controller", "ProductGallery" },
                    { "Action", "ViewProducts" }
                });
            }
        }

    }
}