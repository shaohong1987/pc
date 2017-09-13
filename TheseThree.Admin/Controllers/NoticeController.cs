using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheseThree.Admin.Filters;
using TheseThree.Admin.Models;
using TheseThree.Admin.Models.Entities;
using TheseThree.Admin.Utils;

namespace TheseThree.Admin.Controllers
{
    [Authentication]
    public class NoticeController : BaseController
    {
        // GET: Notice
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetNotice(int limit, int offset, string name)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = NoticeModel.GetNotice(name,user.HospitalId);
                var data = (List<Notice>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelNotice()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string id = Convert.ToString(Request.Form["id"]);
                var result = NoticeModel.DeleteNotice(id, user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
    }
}