using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheseThree.Admin.Filters;
using TheseThree.Admin.Models;
using TheseThree.Admin.Models.Entities;
using TheseThree.Admin.Models.ViewModels;
using TheseThree.Admin.Utils;

namespace TheseThree.Admin.Controllers
{
    [Authentication]
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            var user = GetCurrentUser();
            List<CommonEntityViewModel> models = null;
            if (user != null)
            {
                ViewBag.tag = user.UserType;
                models = new OrganizationModel().GetCommonAttr(user.HospitalId);
            }
            return View(models);
        }

        [HttpGet]
        public JsonResult GetEndUser(int limit, int offset, string name, string phone, string loginId, string deptname,
            string deptcode, string gwcode, string gwname, string zccode, string zcname,string lvcode,string lvname,string decode,string dename,string xzcode,string xzname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = UserModel.GetEndUsers(user.HospitalId, name, phone, loginId, deptname, deptcode, gwcode,
                    gwname, zccode, zcname,lvcode,lvname,decode,dename,xzcode,xzname,user.DeptCode,user.UserType);
                var data = (List<EndUser>) result.Data;
                if (data != null && data.Count > 0)
                    return Json(new {total = data.Count, rows = data.Skip(offset).Take(limit).ToList()},
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new {total = 0, rows = ""}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUser(int limit, int offset,int examid, string name,string loginId, string deptname,
            string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname, string xzcode, string xzname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = UserModel.GetEndUsers(examid,user.HospitalId, name, loginId, deptname, deptcode, gwcode,
                    gwname, zccode, zcname, lvcode, lvname, xzcode, xzname,user.DeptCode,user.UserType);
                var data = (List<EndUser>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserForRole(int limit, int offset, int roleid, string name, string loginId, string deptname,
            string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname, string xzcode, string xzname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = UserModel.GetEndUsersForRole(roleid, user.HospitalId, name, loginId, deptname, deptcode, gwcode,
                    gwname, zccode, zcname, lvcode, lvname, xzcode, xzname);
                var data = (List<EndUser>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEndUsersForTrain(int limit, int offset, int examid, string name, string loginId, string deptname,
            string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname, string xzcode, string xzname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = UserModel.GetEndUsersForTrain(examid, user.HospitalId, name, loginId, deptname, deptcode, gwcode,
                    gwname, zccode, zcname, lvcode, lvname, xzcode, xzname);
                var data = (List<EndUser>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult AddOrUpdateEndUser()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                string name = Request.Form["name"];
                string phone = Request.Form["phone"];
                string loginId = Request.Form["loginId"];
                string deptname = Request.Form["deptname"];
                string deptcode = Request.Form["deptcode"];
                string gwcode = Request.Form["gwcode"];
                string gwname = Request.Form["gwname"];
                string zccode = Request.Form["zccode"];
                string zcname = Request.Form["zcname"];
                string lvcode = Request.Form["lvcode"]; 
                string lvname = Request.Form["lvname"];
                string decode = Request.Form["decode"];
                string dename = Request.Form["dename"];
                string xzcode = Request.Form["xzcode[]"];
                string xzname = Request.Form["xzname[]"];
                if (!UserModel.CheckEndUserExist(phone)||id>0)
                {
                    var result = UserModel.UpdateEndUser(id, name, phone, loginId, deptname, deptcode, gwcode, gwname,
                        zccode, zcname,lvcode,lvname,decode,dename,xzcode,xzname, user.HospitalId, user.HospitalName);
                    if (result.Status != MessageType.Error)
                    {
                        return Json(new {status = "success"}, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new {status = "该名称已存在"}, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new {status = "Error"}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ResetPwd()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Request.Form["id"];
                if (id.StartsWith(","))
                {
                    id = id.Substring(1);
                }
                var result = UserModel.ResetPwd(id, user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    return Json(new {status = "success"}, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new {status = "Error"}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelUser()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string id = Request.Form["id"];
                if (id.StartsWith(","))
                {
                    id = id.Substring(1);
                }
                var result = UserModel.DelUser(id,user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Upload()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var files = Request.Files;
                if (files != null && files.Count > 0)
                {
                    try
                    {
                        var filePath = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        var fileData = files[0];
                        var fileName = Path.GetFileName(fileData.FileName);
                        var fileExtension = Path.GetExtension(fileName);
                        var saveName = Guid.NewGuid() + fileExtension;
                        fileData.SaveAs(filePath + saveName);
                        var list = ExcelHelper.ImportEndUsers(filePath + saveName, user.HospitalId, user.HospitalName);
                        if (list == null)
                        {
                            return Json(new { Success = true, Message = "上传成功" });
                        }
                        return Json(new { Success = false, Message = list });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, Message = "用户身份失效，请重新登陆。" }, JsonRequestBehavior.AllowGet);
        }
    }
}