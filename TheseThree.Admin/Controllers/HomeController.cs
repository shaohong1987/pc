using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using TheseThree.Admin.Filters;
using TheseThree.Admin.Models;
using TheseThree.Admin.Models.Entities;

namespace TheseThree.Admin.Controllers
{
    [Authentication]
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            var user = GetCurrentUser();
            ViewBag.Title = user.HospitalName;
            var hour = DateTime.Now.Hour;
            if (hour >= 6 && hour < 11)
            {
                ViewBag.Hello = "早上好，美好的一天从医护e家开始！";
            }
            if (hour >= 11 && hour < 14)
            {
                ViewBag.Hello = "吃完午饭，再睡个午觉吧！";
            }
            if (hour >= 14 && hour < 19)
            {
                ViewBag.Hello = "下午好，管理员！";
            }
            if (hour >= 19 && hour < 22)
            {
                ViewBag.Hello = "晚上好，管理员！";
            }
            if (hour >= 22 && hour < 24)
            {
                ViewBag.Hello = "夜深了，赶快洗洗睡吧！";
            }
            if (hour >= 0 && hour < 0)
            {
                ViewBag.Hello = "凌晨了，早点儿休息吧！";
            }
            return View();
        }

        public ActionResult Hospital()
        {
            return View();
        }

        public ActionResult Account()
        {
            return View();
        }

        /// <summary>
        /// 护士属性
        /// </summary>
        /// <returns></returns>
        public ActionResult AttributeConfig()
        {
            return View();
        }

        #region 职称
        [HttpGet]
        public JsonResult GetZC(int limit, int offset, string wardname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = new AttributeModel().GetZC(wardname, user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    var data = (List<ZhiChen>)result.Data;
                    if (data != null && data.Count > 0)
                        return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                            JsonRequestBehavior.AllowGet);
                    else
                    {
                        return Json(new { total = -1, rows = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateZC() //
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                string wardname = Request.Form["wardname"];
                var model = new AttributeModel();
                if (!model.CheckZCExist(wardname, user.HospitalId))
                {
                    var result = model.UpdateZC(id, wardname, user.HospitalId);
                    if (result.Status != MessageType.Error)
                    {
                        return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = "该名称已存在" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteZC() // 
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var result = new AttributeModel().DeleteZC(ids, user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 层级
        [HttpGet]
        public JsonResult GetLevel(int limit, int offset, string teamname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = new AttributeModel().GetLevel(teamname, user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    var data = (List<Level>)result.Data;
                    if (data != null && data.Count > 0)
                        return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                            JsonRequestBehavior.AllowGet);
                    else
                    {
                        return Json(new { total = -1, rows = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateLevel() //
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                string wardname = Request.Form["wardname"];
                var model = new AttributeModel();
                if (!model.CheckZCExist(wardname, user.HospitalId))
                {
                    var result = model.UpdateLevel(id, wardname, user.HospitalId);
                    if (result.Status != MessageType.Error)
                    {
                        return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = "该名称已存在" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLevel() // 
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var result = new AttributeModel().DeleteLevel(ids, user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 学历
        [HttpGet]
        public JsonResult GetDegree(int limit, int offset, string teamname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = new AttributeModel().GetDegree(teamname, user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    var data = (List<Degree>)result.Data;
                    if (data != null && data.Count > 0)
                        return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                            JsonRequestBehavior.AllowGet);
                    else
                    {
                        return Json(new { total = -1, rows = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateDegree() //
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                string wardname = Request.Form["wardname"];
                var model = new AttributeModel();
                if (!model.CheckZCExist(wardname, user.HospitalId))
                {
                    var result = model.UpdateDegree(id, wardname, user.HospitalId);
                    if (result.Status != MessageType.Error)
                    {
                        return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = "该名称已存在" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDegree() //  
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var result = new AttributeModel().DeleteDegree(ids, user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 岗位
        [HttpGet]
        public JsonResult GetGW(int limit, int offset, string teamname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = new AttributeModel().GetGW(teamname, user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    var data = (List<GangWei>)result.Data;
                    if (data != null && data.Count > 0)
                        return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                            JsonRequestBehavior.AllowGet);
                    else
                    {
                        return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateGW() //
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                string wardname = Request.Form["wardname"];
                var model = new AttributeModel();
                if (!model.CheckGWExist(wardname, user.HospitalId))
                {
                    var result = model.UpdateGW(id, wardname, user.HospitalId);
                    if (result.Status != MessageType.Error)
                    {
                        return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = "该名称已存在" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteGW() // 
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var result = new AttributeModel().DeleteGW(ids, user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 组织架构     
        public ActionResult Organization()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetWard(int limit, int offset, string wardname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = new OrganizationModel().GetOrganization(wardname, OrganizationType.Ward, user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    var data = (List<Organization>) result.Data;
                    if (data != null && data.Count > 0)
                        return Json(new {total = data.Count, rows = data.Skip(offset).Take(limit).ToList()},
                            JsonRequestBehavior.AllowGet);
                    else
                    {
                        return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new {total = 0, rows = ""}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTeam(int limit, int offset, string wardname)
        { 
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = new OrganizationModel().GetOrganization(wardname, OrganizationType.Team, user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    var data = (List<Organization>)result.Data;
                    if (data != null && data.Count > 0)
                        return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                            JsonRequestBehavior.AllowGet);
                    else
                    {
                        return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateOrg() //
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                string wardname = Request.Form["wardname"];
                int type = Convert.ToInt32(Request.Form["type"]);
                OrganizationType organizationType = type == 0 ? OrganizationType.Ward : OrganizationType.Team;
                var model = new OrganizationModel();
                if (!model.CheckOrganizationExist(wardname, organizationType, user.HospitalId))
                {
                    var result = new OrganizationModel().UpdateOrganization(id, wardname, organizationType,
                        user.HospitalId);
                    if (result.Status != MessageType.Error)
                    {
                        return Json(new {status = "success"}, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new {status = type == 1 ? "该科室名称已存在" : "该组名已存在"}, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new {status = "Error"}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteOrg() // 
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                int type = Convert.ToInt32(Request.Form["type"]);
                OrganizationType organizationType = type == 0 ? OrganizationType.Ward : OrganizationType.Team;
                var result = new OrganizationModel().DeleteOrg(ids, organizationType, user.HospitalId);
                if (result)
                {
                    return Json(new {status = "success"}, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new {status = "Error"}, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}