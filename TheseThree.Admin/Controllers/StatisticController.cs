using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheseThree.Admin.Filters;
using TheseThree.Admin.Models;
using TheseThree.Admin.Models.Entities;

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

        public ActionResult StatisticExam(int id)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                //所有科室
                var list=new OrganizationModel().GetCommonAttr(user.HospitalId);
                if (list.Any())
                {
                    ViewBag.ks = list.Where(it => it.Type == "ks").ToList();
                }
                
                //所有考试月份
                var entities = TeachingModel.GetExamInfo(user.HospitalId);
                if (entities!=null&&entities.Any())
                {
                    ViewBag.monthes = entities.Select(it => it.Value).Distinct().ToList();
                    ViewBag.exams = entities.Select(it => it.Name).Distinct().ToList();
                }
            }
            ViewBag.eid = id;
            return View();
        }

        public JsonResult GetStatisticExam(int limit,int offset,int grade,string month,string id,string deptcode,string loginid,string name)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetExamInfoDetail(grade, month, id, deptcode, loginid, name);
                var data = (List<ExamInfoDetail>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Train()
        {
            return View();
        }

        public ActionResult Notice()
        {
            return View();
        }
    }
}