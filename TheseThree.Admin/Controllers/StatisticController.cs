using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheseThree.Admin.Filters;

namespace TheseThree.Admin.Controllers
{
    [Authentication]
    public class StatisticController : BaseController
    {
        // GET: Statistic
        public ActionResult Index()
        {
            return View();
        }
    }
}