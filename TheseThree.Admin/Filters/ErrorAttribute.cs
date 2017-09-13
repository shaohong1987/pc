using System;
using System.Collections.Generic;
using System.IO;
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
            WriteLog(message, url);
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectToRouteResult("Error", new RouteValueDictionary());
        }

        private void WriteLog(string message,string url)
        {
            string filePath = "C:\\Log\\" + DateTime.Now.ToString("yyyyMMddHH") + ".log";
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (var sw = new StreamWriter(fs))
                {
                  sw.WriteLine("["+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"]-"+"["+message+"]-["+url+"]");  
                }
            }
        }
    }
}