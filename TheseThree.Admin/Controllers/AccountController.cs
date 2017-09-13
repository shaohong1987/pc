using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheseThree.Admin.Filters;
using TheseThree.Admin.Models;
using TheseThree.Admin.Models.Entities;
using TheseThree.Admin.Models.ViewModels;

namespace TheseThree.Admin.Controllers
{
    public class AccountController : BaseController
    {
        [Authentication]
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignIn()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = UserModel.ValidateUser(model);
            if (result.Status == MessageType.Success)
            {
                //在此写入登录日志
                User user = (User) result.Data;
                UserModel.LoginLog(user.UserId, user.UserName, user.HospitalId);
                HttpContext.Session["currentuser"] = result.Data;
                return RedirectToAction("Index", "Home");
            }
            if (result.Status == MessageType.Fail)
            {
                ModelState.AddModelError("", "用户名或密码不正确");
            }
            if (result.Status == MessageType.Error)
            {
                return RedirectToRoute("Error", null);
            }
            return View(model);
        }

        public ActionResult SignOut()
        {
            if (HttpContext.Session != null)
                HttpContext.Session["currentuser"] = null;
            return RedirectToRoute("SignIn");
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult AppDownload()
        {
            return View();
        }
        public ActionResult getUserType()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                return Json(new { userType = user.UserType }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }
    }
}