using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace TheseThree.Admin.Filters
{
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null && filterContext.HttpContext.Session["currentuser"] == null)
                filterContext.Result = new RedirectToRouteResult("SignIn", new RouteValueDictionary());

            base.OnActionExecuting(filterContext);
        }
    }
}