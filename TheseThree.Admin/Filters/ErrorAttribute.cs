using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TheseThree.Admin.Filters
{
    public class ErrorAttribute : ActionFilterAttribute, IExceptionFilter
    {
        /// <summary> 
        /// 异常 
        /// </summary> 
        /// <param name="filterContext"></param> 
        public void OnException(ExceptionContext filterContext)
        {
            Exception error = filterContext.Exception;
            string message = error.Message;
            string url = HttpContext.Current.Request.RawUrl;
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectToRouteResult("Error", new RouteValueDictionary());
        }
    }

}