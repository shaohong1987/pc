using System;
using System.Web.Mvc;
using TheseThree.Admin.Filters;
using TheseThree.Admin.Models.Entities;

namespace TheseThree.Admin.Controllers
{
    [Error]
    public class BaseController : Controller
    {
        public User GetCurrentUser()
        {
            if (HttpContext.Session != null)
            {
                var user = (User)HttpContext.Session["currentuser"];
                return user;
            }
            return null;
        }
    }
}