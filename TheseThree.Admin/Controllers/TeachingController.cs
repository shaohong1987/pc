using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.Office.Interop.Word;
using TheseThree.Admin.Filters;
using TheseThree.Admin.Models;
using TheseThree.Admin.Models.Entities;
using TheseThree.Admin.Models.ViewModels;
using TheseThree.Admin.Utils;

namespace TheseThree.Admin.Controllers
{
    [Authentication]
    public class TeachingController : BaseController
    {
        #region 培训管理

        [Authentication]
        public ActionResult Train()
        {
            return View();
        }

        public ActionResult EditTrain(int id)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var msg = TeachingModel.GetTrain(user.HospitalId, id);
                var test = (Train)msg.Data;
                ViewBag.TrainId = test.Id;
                ViewBag.Time = test.Time;
                ViewBag.Org = test.Org;
                ViewBag.Zhuti = test.Zhuti;
                ViewBag.Address = test.Adress;
                ViewBag.Score = test.Score;
                ViewBag.Teacher = test.Teacher;
                ViewBag.NeedQd = TeachingModel.CheckNeedQd(test.Id);
                var tiku = TeachingModel.GetTiKu(user.HospitalId, "", user.UserType, user.DeptCode);
                if (tiku != null)
                {
                    var tk = (List<TiKu>)tiku.Data;
                    if (tk != null && tk.Count > 0)
                        ViewBag.Tiku = tk;
                }
                var paper = TeachingModel.GetPaper(user.HospitalId);
                ViewBag.Paper = paper;
                var models = new OrganizationModel().GetCommonAttr(user.HospitalId);
                return View(models);
            }
            return null;
        }

        public JsonResult GetKsUser(int limit, int offset, int examid, string name, string loginId, string deptname,
            string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname,
            string xzcode, string xzname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetKsUser(examid, user.HospitalId, name, loginId, deptname, deptcode, gwcode,
                    gwname, zccode, zcname, lvcode, lvname, xzcode, xzname);
                var data = (List<EndUser>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTrain(int limit, int offset, string name, string orgname, string startTime, string endTime)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTrain(name, orgname, startTime, endTime, user.HospitalId);
                var data = (List<Train>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddTrainUser()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                var trainid = Convert.ToInt32(Request.Form["trainid"]);
                var usertype = Convert.ToInt32(Request.Form["usertype"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var result = TeachingModel.UpdateTrainUser(ids, trainid, user.HospitalId, usertype);
                if (result.Status != MessageType.Error)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelTrainQdUser()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var trainid = Convert.ToInt32(Request.Form["trainid"]);
                var result = TeachingModel.DeleteTrainQdUser(id, trainid);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelTrainUser()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                var trainid = Convert.ToInt32(Request.Form["trainid"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var result = TeachingModel.DeleteTrainUser(ids, trainid);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelTrainTiMu()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                var trainid = Convert.ToInt32(Request.Form["trainid"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var result = TeachingModel.DelTrainKhTiMu(ids, trainid);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTrainDetail(int limit, int offset, int examid)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTrainDetail(examid);
                var data = (List<EduKc>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetKhTiMu(int limit, int offset, int trainid)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetKhTiMu(trainid);
                var data = (List<PaperTimu>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEndUsersForTrain(int limit, int offset, int examid, string name, string loginId,
            string deptname,
            string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname,
            string xzcode, string xzname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetEndUsersForTrain(examid, user.HospitalId, name, loginId, deptname,
                    deptcode, gwcode,
                    gwname, zccode, zcname, lvcode, lvname, xzcode, xzname);
                var data = (List<EndUser>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddTrainKhTiMu()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                var trainid = Convert.ToInt32(Request.Form["trainid"]);
                var cent = 0;
                if (!string.IsNullOrEmpty(Request.Form["cent"]))
                {
                    cent = Convert.ToInt32(Request.Form["cent"]);
                }
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var result = TeachingModel.AddTrainKhTiMu(ids, trainid, cent);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOrUpdateTrainTiMu()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var trainid = Convert.ToInt32(Request.Form["trainid"]);
                var anli = Convert.ToString(Request.Form["anli"]);
                var remark = Convert.ToString(Request.Form["remark"]);
                var question = Convert.ToString(Request.Form["question"]);
                var difficulty = Convert.ToSingle(Request.Form["difficulty"]);
                var label = Convert.ToInt32(Request.Form["label"]);
                var type = Convert.ToString(Request.Form["type"]);
                var itema = Convert.ToString(Request.Form["itema"]);
                var itemaC = Convert.ToInt32(Request.Form["itema_c"]);
                var itemb = Convert.ToString(Request.Form["itemb"]);
                var itembC = Convert.ToInt32(Request.Form["itemb_c"]);
                var itemc = Convert.ToString(Request.Form["itemc"]);
                var itemcC = Convert.ToInt32(Request.Form["itemc_c"]);
                var itemd = Convert.ToString(Request.Form["itemd"]);
                var itemdC = Convert.ToInt32(Request.Form["itemd_c"]);
                var iteme = Convert.ToString(Request.Form["iteme"]);
                var itemeC = Convert.ToInt32(Request.Form["iteme_c"]);
                var itemf = Convert.ToString(Request.Form["itemf"]);
                var itemfC = Convert.ToInt32(Request.Form["itemf_c"]);
                var itemg = Convert.ToString(Request.Form["itemg"]);
                var itemgC = Convert.ToInt32(Request.Form["itemg_c"]);
                var itemh = Convert.ToString(Request.Form["itemh"]);
                var itemhC = Convert.ToInt32(Request.Form["itemh_c"]);
                var itemi = Convert.ToString(Request.Form["itemi"]);
                var itemiC = Convert.ToInt32(Request.Form["itemi_c"]);
                var itemj = Convert.ToString(Request.Form["itemj"]);
                var itemjC = Convert.ToInt32(Request.Form["itemj_c"]);
                var result = TeachingModel.UpdateTrainTiMu(id, anli, remark, question, label, type, itema, itemaC,
                    itemb, itembC, itemc, itemcC, itemd, itemdC, iteme, itemeC, itemf, itemfC, itemg, itemgC,
                    itemh, itemhC, itemi, itemiC, itemj, itemjC, trainid, difficulty, user.HospitalId);
                if (result.Status != MessageType.Error)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelTrainKhTiMu()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["ids"]);
                var trainid = Convert.ToInt32(Request.Form["trainid"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var result = TeachingModel.DelTrainKhTiMu(ids, trainid);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelTrainDetail()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var examid = Convert.ToInt32(Request.Form["examid"]);
                var result = TeachingModel.DelTrainDetail(id, examid);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTrain()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var zhuti = Convert.ToString(Request.Form["zhuti"]);
                var trainid = Convert.ToInt32(Request.Form["trainid"]);
                var org = Convert.ToString(Request.Form["org"]);
                var time = Convert.ToString(Request.Form["time"]);
                var teacher = Convert.ToString(Request.Form["teacher"]);
                var address = Convert.ToString(Request.Form["address"]);
                var score = Convert.ToInt32(Request.Form["score"]);
                var level = Convert.ToString(Request.Form["level"]);
                var style = Convert.ToString(Request.Form["style"]);
                var apppush = Convert.ToInt32(Request.Form["apppush"]) > 0;
                var smspush = Convert.ToInt32(Request.Form["smspush"]) > 0;
                var result = TeachingModel.UpdateTrain(trainid, zhuti, address, org, time, teacher, score, level,
                    user.HospitalId, style, apppush, smspush);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTrainDetail()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var title = Convert.ToString(Request.Form["title"]);
                var trainid = Convert.ToInt32(Request.Form["trainid"]);
                var tec = Convert.ToString(Request.Form["tec"]);
                var result = TeachingModel.UpdateTrainDetail(trainid, title, tec);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelTrainFj()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var examid = Convert.ToInt32(Request.Form["examid"]);
                var result = TeachingModel.DelTrainFj(id, examid);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTrainFj(int limit, int offset, int examid)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTrainFj(examid);
                var data = (List<FuJian>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadFuJian()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var files = Request.Files;
                if (files != null && files.Count > 0)
                    try
                    {
                        var trainid = Convert.ToInt32(Request.Form["trainid"]);
                        var kjname = Convert.ToString(Request.Form["kjname"]);
                        //string filePath = @"E:\jboss-as-7.1.1.Final\welcome-content\kj\" + user.HospitalId + "\\";
                        var filePath = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);
                        var fileData = files[0];
                        if (fileData != null)
                        {
                            var fileName = Path.GetFileName(fileData.FileName);
                            var fileExtension = Path.GetExtension(fileName);
                            var saveName = Guid.NewGuid() + fileExtension;
                            fileData.SaveAs(filePath + saveName);
                            TeachingModel.AddTrainFj(trainid, kjname, saveName, 0);
                        }
                        return Json(new { Success = true });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, Message = "用户身份失效，请重新登陆。" }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public JsonResult UploadNr()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var trainid = Convert.ToInt32(Request.Form["trainid"]);
                var nrname = Convert.ToString(Request.Form["nrname"]);
                var content = Convert.ToString(Request.Form["content"]);
                //string filePath = @"E:\jboss-as-7.1.1.Final\welcome-content\kj\" + user.HospitalId + "\\";
                var filePath = Server.MapPath("~/Uploads/");
                var filename = IoHelper.CreateFile(filePath, content);
                var result = TeachingModel.AddTrainNeiRong(trainid, nrname, filename);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserForTrain(int limit, int offset, int trainid)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetEndUserForTrain(trainid);
                var data = (List<EndUser>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 题库管理

        public ActionResult ShareSubject()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var tkid = Convert.ToInt32(Request.Form["tkid"]);
                var deptcodes = Request.Form["sectionids[]"];
                var result = TeachingModel.ShareTiKu(tkid, deptcodes, user.HospitalId);
                if (result.Status != MessageType.Error)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public ActionResult Subject()
        {
            var user = GetCurrentUser();
            ViewBag.tag = user.UserType;
            var models = new OrganizationModel().GetCommonAttr(user.HospitalId);
            return View(models);
        }

        [Authentication]
        public JsonResult GetTiKu(int limit, int offset, string name)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTiKu(user.HospitalId, name, user.UserType, user.DeptCode);
                var data = (List<TiKu>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiMu(int limit, int offset, int section, string name, string type, string labelname,
            string labelcode)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTiMu(user.HospitalId, name, type, labelname, labelcode, section);
                var data = (List<TiMu>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "[]" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiMus(int limit, int offset, int paperid, int section, string name, string labelname,
            string labelcode, int exerciseType)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTiMu(paperid, user.HospitalId, name, labelname, labelcode, section,
                    exerciseType);
                var data = (List<TiMu>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiMuForTrain(int limit, int offset, int section, string name, string labelname,
            string labelcode, int exerciseType)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTiMuForTrain(user.HospitalId, name, labelname, labelcode, section,
                    exerciseType);
                var data = (List<TiMu>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }


        [Authentication]
        public JsonResult AddOrUpdateSubject()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var name = Request.Form["name"];
                var result = TeachingModel.UpdateTiKu(id, name, user.HospitalId, user.UserName, user.DeptCode);
                if (result.Status != MessageType.Error)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult DelTiKu()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var result = TeachingModel.DeleteTk(ids, user.HospitalId);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public ActionResult EditSection(int id)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                if (id < 1)
                    return null;
                var list = TeachingModel.GetLabel(id, user.HospitalId);
                ViewBag.SectionId = id;
                return View(list);
            }
            return null;
        }

        [Authentication]
        public JsonResult UploadSubject(int sectionId)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var files = Request.Files;
                if (files != null && files.Count > 0)
                    try
                    {
                        var filePath = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);
                        var fileData = files[0];
                        if (fileData != null)
                        {
                            var fileName = Path.GetFileName(fileData.FileName);
                            var fileExtension = Path.GetExtension(fileName);
                            var saveName = Guid.NewGuid() + fileExtension;
                            fileData.SaveAs(filePath + saveName);
                            var labels = TeachingModel.GetLabels(sectionId, user.HospitalId);
                            var list = ExcelHelper.ImportSubject(filePath + saveName, user.HospitalId, sectionId, labels);
                            if (list == null)
                                return Json(new { Success = true, Message = "上传成功" });
                            return Json(new { Success = false, Message = list });
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, Message = "用户身份失效，请重新登陆。" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult AddOrUpdateTiMu()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var sectionId = Convert.ToInt32(Request.Form["sectionId"]);
                var anli = Convert.ToString(Request.Form["anli"]);
                var remark = Convert.ToString(Request.Form["remark"]);
                var question = Convert.ToString(Request.Form["question"]);
                var difficulty = Convert.ToSingle(Request.Form["difficulty"]);
                var label = Convert.ToString(Request.Form["label"]);
                var type = Convert.ToString(Request.Form["type"]);
                var itema = Convert.ToString(Request.Form["itema"]);
                var itemaC = Convert.ToInt32(Request.Form["itema_c"]);
                var itemb = Convert.ToString(Request.Form["itemb"]);
                var itembC = Convert.ToInt32(Request.Form["itemb_c"]);
                var itemc = Convert.ToString(Request.Form["itemc"]);
                var itemcC = Convert.ToInt32(Request.Form["itemc_c"]);
                var itemd = Convert.ToString(Request.Form["itemd"]);
                var itemdC = Convert.ToInt32(Request.Form["itemd_c"]);
                var iteme = Convert.ToString(Request.Form["iteme"]);
                var itemeC = Convert.ToInt32(Request.Form["iteme_c"]);
                var itemf = Convert.ToString(Request.Form["itemf"]);
                var itemfC = Convert.ToInt32(Request.Form["itemf_c"]);
                var itemg = Convert.ToString(Request.Form["itemg"]);
                var itemgC = Convert.ToInt32(Request.Form["itemg_c"]);
                var itemh = Convert.ToString(Request.Form["itemh"]);
                var itemhC = Convert.ToInt32(Request.Form["itemh_c"]);
                var itemi = Convert.ToString(Request.Form["itemi"]);
                var itemiC = Convert.ToInt32(Request.Form["itemi_c"]);
                var itemj = Convert.ToString(Request.Form["itemj"]);
                var itemjC = Convert.ToInt32(Request.Form["itemj_c"]);
                var result = TeachingModel.UpdateTiMu(id, anli, remark, question, label, type, itema, itemaC, itemb,
                    itembC, itemc, itemcC, itemd, itemdC, iteme, itemeC, itemf, itemfC, itemg, itemgC, itemh,
                    itemhC, itemi, itemiC, itemj, itemjC, sectionId, difficulty, user.HospitalId);
                if (result.Status != MessageType.Error)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult DelTiMu()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var sectionid = Convert.ToInt32(Request.Form["sectionId"]);
                var result = TeachingModel.DeleteTm(ids, sectionid);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 试卷管理

        [Authentication]
        public ActionResult Paper()
        {
            return View();
        }

        [Authentication]
        public JsonResult GetPaper(int limit, int offset, string name)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetPaper(user.HospitalId, name, user.DeptCode, user.UserType);
                var data = (List<Paper>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult GetPaperTimu(int paperid, int exerciseType)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetPaperTimu(user.HospitalId, paperid, exerciseType);
                var data = (List<PaperTimu>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult GetPQuestionStas()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var paperId = Convert.ToInt32(Request.Form["paperId"]);
                var result = TeachingModel.GetPQuestionStas(paperId, user.HospitalId);
                return Json(new { status = "success", data = result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult AddOrUpdatePaper()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var name = Convert.ToString(Request.Form["name"]);
                var deptcode = Convert.ToInt32(Request.Form["deptcode"]);
                var deptName = Convert.ToString(Request.Form["deptName"]);
                var result = TeachingModel.UpdatePaper(id, name, deptcode, deptName, user.HospitalId);
                if (result.Status != MessageType.Error)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult AddPaperQuestion()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var exerciseType = Convert.ToInt32(Request.Form["exerciseType"]);
                var paperid = Convert.ToInt32(Request.Form["paperid"]);
                var cent = 0;
                if (!string.IsNullOrEmpty(Request.Form["cent"]))
                    cent = Convert.ToInt32(Request.Form["cent"]);
                var result = TeachingModel.UpdatePaperQuestion(ids, exerciseType, paperid, user.HospitalId, cent);
                if (result.Status != MessageType.Error)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult DelPaperQuestion()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var exerciseType = Convert.ToInt32(Request.Form["exerciseType"]);
                var paperid = Convert.ToInt32(Request.Form["paperid"]);
                var result = TeachingModel.DelPaperQuestion(id, paperid, exerciseType, user.HospitalId);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult DelPaper()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var result = TeachingModel.DeletePaper(ids, user.HospitalId);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public ActionResult PaperEdit(int id)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var msg = TeachingModel.GetPaper(user.HospitalId, id);
                var paper = (Paper)msg.Data;
                ViewBag.PaperId = paper.Id;
                ViewBag.PaperName = paper.Name;
                ViewBag.TotalMini = paper.Duration;
                var tiku = TeachingModel.GetTiKu(user.HospitalId, "", user.UserType, user.DeptCode);
                var tk = (List<TiKu>)tiku?.Data;
                if (tk != null && tk.Count > 0)
                    ViewBag.Tiku = tk;
                return View();
            }
            return null;
        }

        public JsonResult AddOrUpdateExamTiMu()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var paperid = Convert.ToInt32(Request.Form["paperid"]);
                var anli = Convert.ToString(Request.Form["anli"]);
                var remark = Convert.ToString(Request.Form["remark"]);
                var question = Convert.ToString(Request.Form["question"]);
                var difficulty = 0.0f;
                if (Request.Form["difficulty"] != null)
                {
                    difficulty = Convert.ToSingle(Request.Form["difficulty"]);
                }

                var label = Convert.ToInt32(Request.Form["label"]);
                var type = Convert.ToString(Request.Form["type"]);
                var itema = Convert.ToString(Request.Form["itema"]);
                var itemaC = Convert.ToInt32(Request.Form["itema_c"]);
                var itemb = Convert.ToString(Request.Form["itemb"]);
                var itembC = Convert.ToInt32(Request.Form["itemb_c"]);
                var itemc = Convert.ToString(Request.Form["itemc"]);
                var itemcC = Convert.ToInt32(Request.Form["itemc_c"]);
                var itemd = Convert.ToString(Request.Form["itemd"]);
                var itemdC = Convert.ToInt32(Request.Form["itemd_c"]);
                var iteme = Convert.ToString(Request.Form["iteme"]);
                var itemeC = Convert.ToInt32(Request.Form["iteme_c"]);
                var itemf = Convert.ToString(Request.Form["itemf"]);
                var itemfC = Convert.ToInt32(Request.Form["itemf_c"]);
                var itemg = Convert.ToString(Request.Form["itemg"]);
                var itemgC = Convert.ToInt32(Request.Form["itemg_c"]);
                var itemh = Convert.ToString(Request.Form["itemh"]);
                var itemhC = Convert.ToInt32(Request.Form["itemh_c"]);
                var itemi = Convert.ToString(Request.Form["itemi"]);
                var itemiC = Convert.ToInt32(Request.Form["itemi_c"]);
                var itemj = Convert.ToString(Request.Form["itemj"]);
                var itemjC = Convert.ToInt32(Request.Form["itemj_c"]);
                var result = TeachingModel.UpdateExamTiMu(id, anli, remark, question, label, type, itema, itemaC,
                    itemb, itembC, itemc, itemcC, itemd, itemdC, iteme, itemeC, itemf, itemfC, itemg, itemgC,
                    itemh, itemhC, itemi, itemiC, itemj, itemjC, paperid, difficulty, user.HospitalId);
                if (result.Status != MessageType.Error)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLabels()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["sectionId"]);
                var result = TeachingModel.GetLabels(id, user.HospitalId);
                return Json(new { status = "success", data = result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult SavePaper()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var paperId = Convert.ToInt32(Request.Form["paperid"]);
                var papername = Convert.ToString(Request.Form["papername"]);
                var totalMini = Convert.ToInt32(Request.Form["totalMini"]);
                var result = TeachingModel.SavePaper(paperId, papername, totalMini, user.HospitalId, user.DeptCode);
                return Json(new { status = "success", data = result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 考试管理

        [Authentication]
        public ActionResult Exam()
        {
            return View();
        }

        public JsonResult GetExam(int limit, int offset, string name, string startTime, string endTime)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetExam(user.HospitalId, name, startTime, endTime, user.DeptCode,
                    user.UserType);
                var data = (List<Test>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExamEdit(int id)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var msg = TeachingModel.GetExam(user.HospitalId, id);
                var test = (Test)msg.Data;
                ViewBag.ExamId = test.Id;
                ViewBag.Begintime = test.Begintime;
                ViewBag.Endtime = test.Endtime;
                ViewBag.Title = test.Title;
                ViewBag.Address = test.Adress;
                ViewBag.Fen = test.Fen;
                ViewBag.Jigescore = test.Jigescore;
                ViewBag.PaperId = test.PaperId;
                ViewBag.Grade = test.Grade;
                var paper = TeachingModel.GetPaper(user.HospitalId);
                ViewBag.Paper = paper;
                var models = new OrganizationModel().GetCommonAttr(user.HospitalId);
                return View(models);
            }
            return null;
        }

        public JsonResult AddExamUser()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                var examid = Convert.ToInt32(Request.Form["examid"]);
                var usertype = Convert.ToInt32(Request.Form["usertype"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var result = TeachingModel.UpdateExamUser(ids, examid, usertype, user.HospitalId);
                if (result.Status != MessageType.Error)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserForExam(int limit, int offset, int examId)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetEndUserForExam(examId);
                var data = (List<EndUser>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelExamUser()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var userType = Convert.ToInt32(Request.Form["userType"]);
                var examid = Convert.ToInt32(Request.Form["examid"]);
                var result = TeachingModel.DeleteExamUser(id, userType, examid);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetKsUserForExam(int limit, int offset, int examid, string name, string loginId,
            string deptname,
            string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname,
            string xzcode, string xzname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetKsUserForExam(examid, user.HospitalId, name, loginId, deptname, deptcode,
                    gwcode,
                    gwname, zccode, zcname, lvcode, lvname, xzcode, xzname);
                var data = (List<EndUser>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveExam()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var examname = Convert.ToString(Request.Form["examname"]);
                var examid = Convert.ToInt32(Request.Form["examid"]);
                var paper = Convert.ToString(Request.Form["paper"]);
                var startTime = Convert.ToString(Request.Form["startTime"]);
                var endTime = Convert.ToString(Request.Form["endTime"]);
                var jigescroe = Convert.ToString(Request.Form["jigescroe"]);
                var address = Convert.ToString(Request.Form["address"]);
                var grade = Convert.ToString(Request.Form["grade"]);
                var apppush = Convert.ToInt32(Request.Form["apppush"]) > 0;
                var smspush = Convert.ToInt32(Request.Form["smspush"]) > 0;
                var result = TeachingModel.UpdateExam(examid, examname, address, startTime, endTime, paper, jigescroe,
                    user.HospitalId, grade, apppush, smspush, user.DeptCode);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelExam()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var result = TeachingModel.DelExam(ids, user.HospitalId);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelTrain()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                var result = TeachingModel.DelTrain(ids, user.HospitalId);
                if (result)
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportExam()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var paperid = Convert.ToInt32(Request.Form["id"]);
                //string name = Convert.ToString(Request.Form["name"]);
                var result = TeachingModel.GetExamTimu(user.HospitalId, paperid);
                var data = (List<PaperTimu>)result.Data;
                var msg = TeachingModel.GetExam(user.HospitalId, paperid);
                var test = (Test)msg.Data;
                try
                {
                    object nothing = Missing.Value;
                    //创建Word文档
                    Application wordApp = new ApplicationClass();
                    var wordDoc = wordApp.Documents.Add(ref nothing, ref nothing, ref nothing, ref nothing);
                    //添加页眉
                    wordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;
                    wordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;
                    wordApp.ActiveWindow.ActivePane.Selection.InsertAfter(
                        user.HospitalName + "       " + test.Begintime);
                    wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; //设置右对齐
                    wordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument; //跳出页眉设置

                    wordApp.Selection.ParagraphFormat.LineSpacing = 15f; //设置文档的行间距
                    wordDoc.Paragraphs.Last.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    wordDoc.Paragraphs.Last.Range.Font.Size = 15;
                    wordDoc.Paragraphs.Last.Range.Font.Bold = 1;
                    wordDoc.Paragraphs.Last.Range.Text = test.Title + "\n";
                    wordDoc.Paragraphs.Last.Range.Font.Bold = 0;
                    wordDoc.Paragraphs.Last.Range.Font.Size = 11;
                    wordDoc.Paragraphs.Last.Range.Text = "科室：             姓名：               得分：" + "\n";
                    wordDoc.Paragraphs.Last.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    if (data != null)
                    {
                        var i = 1;
                        var j = 1;
                        var k = 1;
                        var m = 1;
                        var danxuan = new List<PaperTimu>();
                        var duoxuan = new List<PaperTimu>();
                        var panduan = new List<PaperTimu>();
                        var anli = new List<PaperTimu>();
                        foreach (var timu in data)
                            if (timu.ExerciseType == 1 && string.IsNullOrEmpty(timu.Anli))
                                danxuan.Add(timu);
                            else if (timu.ExerciseType == 2 && string.IsNullOrEmpty(timu.Anli))
                                duoxuan.Add(timu);
                            else if (timu.ExerciseType == 3 && string.IsNullOrEmpty(timu.Anli))
                                panduan.Add(timu);
                            else
                                anli.Add(timu);
                        if (panduan.Count > 0)
                        {
                            wordDoc.Paragraphs.Last.Range.Text =
                                "一、判断题： （每题" + panduan[0].Cent + "分   共" + panduan[0].Cent * panduan.Count + "分）" +
                                "\n";
                            foreach (var timu in panduan)
                            {
                                wordDoc.Paragraphs.Last.Range.Text = i + "、" + timu.Question + "   (  )" + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = timu.ItemA + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = timu.ItemB + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = timu.ItemC + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = timu.ItemD + "\n";
                                i++;
                            }
                        }

                        if (danxuan.Count > 0)
                        {
                            if (panduan.Count > 0)
                                wordDoc.Paragraphs.Last.Range.Text =
                                    "二、单选题： （每题" + danxuan[0].Cent + "分   共" + danxuan[0].Cent * danxuan.Count + "分）" +
                                    "\n";
                            else
                                wordDoc.Paragraphs.Last.Range.Text =
                                    "一、单选题： （每题" + danxuan[0].Cent + "分   共" + danxuan[0].Cent * danxuan.Count + "分）" +
                                    "\n";
                            foreach (var timu in danxuan)
                            {
                                wordDoc.Paragraphs.Last.Range.Text = j + "、" + timu.Question + "   (  )" + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = "A." + timu.ItemA + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = "B." + timu.ItemB + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemC))
                                    wordDoc.Paragraphs.Last.Range.Text = "C." + timu.ItemC + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemD))
                                    wordDoc.Paragraphs.Last.Range.Text = "D." + timu.ItemD + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemE))
                                    wordDoc.Paragraphs.Last.Range.Text = "E." + timu.ItemE + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemF))
                                    wordDoc.Paragraphs.Last.Range.Text = "F." + timu.ItemF + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemG))
                                    wordDoc.Paragraphs.Last.Range.Text = "G." + timu.ItemG + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemH))
                                    wordDoc.Paragraphs.Last.Range.Text = "H." + timu.ItemH + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemI))
                                    wordDoc.Paragraphs.Last.Range.Text = "I." + timu.ItemI + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemJ))
                                    wordDoc.Paragraphs.Last.Range.Text = "J." + timu.ItemJ + "\n";
                                j++;
                            }
                        }
                        if (duoxuan.Count > 0)
                        {
                            if (panduan.Count > 0)
                            {
                                if (danxuan.Count > 0)
                                    wordDoc.Paragraphs.Last.Range.Text =
                                        "三、多选题： （每题" + duoxuan[0].Cent + "分   共" + duoxuan[0].Cent * duoxuan.Count +
                                        "分）" + "\n";
                                else
                                    wordDoc.Paragraphs.Last.Range.Text =
                                        "二、多选题： （每题" + duoxuan[0].Cent + "分   共" + duoxuan[0].Cent * duoxuan.Count +
                                        "分）" + "\n";
                            }
                            else
                            {
                                if (danxuan.Count > 0)
                                    wordDoc.Paragraphs.Last.Range.Text =
                                        "二、多选题： （每题" + duoxuan[0].Cent + "分   共" + duoxuan[0].Cent * duoxuan.Count +
                                        "分）" + "\n";
                                else
                                    wordDoc.Paragraphs.Last.Range.Text =
                                        "一、多选题： （每题" + duoxuan[0].Cent + "分   共" + duoxuan[0].Cent * duoxuan.Count +
                                        "分）" + "\n";
                            }
                            foreach (var timu in duoxuan)
                            {
                                wordDoc.Paragraphs.Last.Range.Text = k + "、" + timu.Question + "   (  )" + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = "A." + timu.ItemA + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = "B." + timu.ItemB + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemC))
                                    wordDoc.Paragraphs.Last.Range.Text = "C." + timu.ItemC + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemD))
                                    wordDoc.Paragraphs.Last.Range.Text = "D." + timu.ItemD + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemE))
                                    wordDoc.Paragraphs.Last.Range.Text = "E." + timu.ItemE + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemF))
                                    wordDoc.Paragraphs.Last.Range.Text = "F." + timu.ItemF + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemG))
                                    wordDoc.Paragraphs.Last.Range.Text = "G." + timu.ItemG + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemH))
                                    wordDoc.Paragraphs.Last.Range.Text = "H." + timu.ItemH + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemI))
                                    wordDoc.Paragraphs.Last.Range.Text = "I." + timu.ItemI + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemJ))
                                    wordDoc.Paragraphs.Last.Range.Text = "J." + timu.ItemJ + "\n";
                                k++;
                            }
                        }
                        if (anli.Count > 0)
                        {
                            if (duoxuan.Count > 0)
                            {
                                if (panduan.Count > 0)
                                {
                                    if (danxuan.Count > 0)
                                        wordDoc.Paragraphs.Last.Range.Text = "四、案例题：" + "\n";
                                    else
                                        wordDoc.Paragraphs.Last.Range.Text = "三、案例题：" + "\n";
                                }
                                else
                                {
                                    if (danxuan.Count > 0)
                                        wordDoc.Paragraphs.Last.Range.Text = "三、案例题：" + "\n";
                                    else
                                        wordDoc.Paragraphs.Last.Range.Text = "二、案例题：" + "\n";
                                }
                            }
                            else
                            {
                                if (panduan.Count > 0)
                                {
                                    if (danxuan.Count > 0)
                                        wordDoc.Paragraphs.Last.Range.Text = "三、案例题：" + "\n";
                                    else
                                        wordDoc.Paragraphs.Last.Range.Text = "二、案例题：" + "\n";
                                }
                                else
                                {
                                    if (danxuan.Count > 0)
                                        wordDoc.Paragraphs.Last.Range.Text = "二、案例题：" + "\n";
                                    else
                                        wordDoc.Paragraphs.Last.Range.Text = "一、案例题：" + "\n";
                                }
                            }
                            var anlistr = new List<string>();
                            foreach (var timu in anli)
                            {
                                if (anlistr.Count > 0 && anlistr.Contains(timu.Anli))
                                    continue;
                                anlistr.Add(timu.Anli);
                            }
                            foreach (var t in anlistr)
                            {
                                wordDoc.Paragraphs.Last.Range.Text = "案例：  " + t + "\n";
                                foreach (var timu in anli)
                                    if (timu.Anli.Equals(t))
                                    {
                                        if (timu.ExerciseType == 1)
                                            wordDoc.Paragraphs.Last.Range.Text =
                                                m + "、【单选题】" + timu.Question + "   (  )" + "\n";
                                        if (timu.ExerciseType == 2)
                                            wordDoc.Paragraphs.Last.Range.Text =
                                                m + "、【多选题】" + timu.Question + "   (  )" + "\n";
                                        if (timu.ExerciseType == 3)
                                            wordDoc.Paragraphs.Last.Range.Text =
                                                m + "、【判断题】" + timu.Question + "   (  )" + "\n";
                                        wordDoc.Paragraphs.Last.Range.Text = "A." + timu.ItemA + "\n";
                                        wordDoc.Paragraphs.Last.Range.Text = "B." + timu.ItemB + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemC))
                                            wordDoc.Paragraphs.Last.Range.Text = "C." + timu.ItemC + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemD))
                                            wordDoc.Paragraphs.Last.Range.Text = "D." + timu.ItemD + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemE))
                                            wordDoc.Paragraphs.Last.Range.Text = "E." + timu.ItemE + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemF))
                                            wordDoc.Paragraphs.Last.Range.Text = "F." + timu.ItemF + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemG))
                                            wordDoc.Paragraphs.Last.Range.Text = "G." + timu.ItemG + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemH))
                                            wordDoc.Paragraphs.Last.Range.Text = "H." + timu.ItemH + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemI))
                                            wordDoc.Paragraphs.Last.Range.Text = "I." + timu.ItemI + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemJ))
                                            wordDoc.Paragraphs.Last.Range.Text = "J." + timu.ItemJ + "\n";
                                        m++;
                                    }
                            }
                        }
                        wordDoc.Paragraphs.Last.Range.InsertBreak();
                        wordDoc.Paragraphs.Last.Range.Text = "答题卡：\n";
                        object unite = WdUnits.wdStory;
                        wordDoc.Content.InsertAfter("\n"); //这一句与下一句的顺序不能颠倒，原因还没搞透
                        wordApp.Selection.EndKey(ref unite, ref nothing); //将光标移动到文档末尾
                        wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        if (panduan.Count > 0)
                        {
                            wordDoc.Paragraphs.Last.Range.Text = "判断题\n";
                            wordApp.Selection.EndKey(ref unite, ref nothing);
                            var tableRow1 = (i / 15 + 1) * 2;
                            var tableColumn1 = i <= 15 ? i : 15;
                            //定义一个Word中的表格对象
                            var table1 = wordDoc.Tables.Add(wordApp.Selection.Range,
                                tableRow1, tableColumn1, ref nothing, ref nothing); //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
                            table1.Borders.Enable = 1; //这个值可以设置得很大，例如5、13等等
                            table1.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                            var count1 = 1;
                            for (var w = 1; w < tableRow1; w++)
                                if (w % 2 != 0)
                                    for (var e = 1; e <= tableColumn1; e++)
                                    {
                                        if (count1 < i)
                                            table1.Cell(w, e).Range.Text = "" + count1; //填充每列的标题
                                        count1++;
                                    }
                        }
                        if (danxuan.Count > 0)
                        {
                            wordDoc.Paragraphs.Last.Range.Text = "单选题\n";
                            wordApp.Selection.EndKey(ref unite, ref nothing);
                            var tableRow2 = (j / 15 + 1) * 2;
                            var tableColumn2 = j <= 15 ? j : 15;
                            //定义一个Word中的表格对象
                            var table2 = wordDoc.Tables.Add(wordApp.Selection.Range,
                                tableRow2, tableColumn2, ref nothing, ref nothing); //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
                            table2.Borders.Enable = 1; //这个值可以设置得很大，例如5、13等等
                            table2.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                            var count2 = 1;
                            for (var w = 1; w <= tableRow2; w++)
                                if (w % 2 != 0)
                                    for (var e = 1; e <= tableColumn2; e++)
                                    {
                                        if (count2 < j)
                                            table2.Cell(w, e).Range.Text = "" + count2; //填充每列的标题
                                        count2++;
                                    }
                        }
                        if (duoxuan.Count > 0)
                        {
                            wordDoc.Paragraphs.Last.Range.Text = "多选题\n";
                            wordApp.Selection.EndKey(ref unite, ref nothing);
                            var tableRow3 = (k / 15 + 1) * 2;
                            var tableColumn3 = k <= 15 ? k : 15;
                            //定义一个Word中的表格对象
                            var table3 = wordDoc.Tables.Add(wordApp.Selection.Range,
                                tableRow3, tableColumn3, ref nothing, ref nothing); //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
                            table3.Borders.Enable = 1; //这个值可以设置得很大，例如5、13等等
                            table3.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                            var count3 = 1;
                            for (var w = 1; w < tableRow3; w++)
                                if (w % 2 != 0)
                                    for (var e = 1; e <= tableColumn3; e++)
                                    {
                                        if (count3 < k)
                                            table3.Cell(w, e).Range.Text = "" + count3; //填充每列的标题
                                        count3++;
                                    }
                        }
                        if (anli.Count > 0)
                        {
                            wordDoc.Paragraphs.Last.Range.Text = "案例题\n";
                            wordApp.Selection.EndKey(ref unite, ref nothing);
                            var tableRow4 = (m / 15 + 1) * 2;
                            int tableColumn4 = m <= 15 ? m : 15;
                            //定义一个Word中的表格对象
                            var table4 = wordDoc.Tables.Add(wordApp.Selection.Range,
                                tableRow4, tableColumn4, ref nothing, ref nothing); //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
                            table4.Borders.Enable = 1; //这个值可以设置得很大，例如5、13等等
                            table4.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                            var count4 = 1;
                            for (var w = 1; w < tableRow4; w++)
                                if (w % 2 != 0)
                                    for (var e = 1; e <= tableColumn4; e++)
                                    {
                                        if (count4 < m)
                                            table4.Cell(w, e).Range.Text = "" + count4; //填充每列的标题
                                        count4++;
                                    }
                        }
                        wordDoc.Paragraphs.Last.Range.InsertBreak();
                        wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        wordDoc.Paragraphs.Last.Range.Text = "标准答案：\n";
                        wordDoc.Content.InsertAfter("\n"); //这一句与下一句的顺序不能颠倒，原因还没搞透
                        wordApp.Selection.EndKey(ref unite, ref nothing); //将光标移动到文档末尾
                        wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        if (panduan.Count > 0)
                        {
                            wordDoc.Paragraphs.Last.Range.Text = "判断题\n";
                            wordApp.Selection.EndKey(ref unite, ref nothing);
                            var tableRow1 = (i / 15 + 1) * 2;
                            var tableColumn1 = i <= 15 ? i : 15;
                            //定义一个Word中的表格对象
                            var table1 = wordDoc.Tables.Add(wordApp.Selection.Range,
                                tableRow1, tableColumn1, ref nothing, ref nothing); //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
                            table1.Borders.Enable = 1; //这个值可以设置得很大，例如5、13等等
                            table1.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                            var count1 = 1;
                            for (var w = 1; w < tableRow1; w++)
                                if (w % 2 != 0)
                                    for (var e = 1; e <= tableColumn1; e++)
                                    {
                                        if (count1 < i)
                                        {
                                            table1.Cell(w, e).Range.Text = "" + count1; //填充每列的标题
                                            table1.Cell(w + 1, e).Range.Text = panduan[count1 - 1].Answer;
                                        }

                                        count1++;
                                    }
                        }
                        if (danxuan.Count > 0)
                        {
                            wordDoc.Paragraphs.Last.Range.Text = "单选题\n";
                            wordApp.Selection.EndKey(ref unite, ref nothing);
                            var tableRow2 = (j / 15 + 1) * 2;
                            var tableColumn2 = j <= 15 ? j : 15;
                            //定义一个Word中的表格对象
                            var table2 = wordDoc.Tables.Add(wordApp.Selection.Range,
                                tableRow2, tableColumn2, ref nothing, ref nothing); //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
                            table2.Borders.Enable = 1; //这个值可以设置得很大，例如5、13等等
                            table2.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                            var count2 = 1;
                            for (var w = 1; w <= tableRow2; w++)
                                if (w % 2 != 0)
                                    for (var e = 1; e <= tableColumn2; e++)
                                    {
                                        if (count2 < j)
                                        {
                                            table2.Cell(w, e).Range.Text = "" + count2; //填充每列的标题
                                            table2.Cell(w + 1, e).Range.Text = danxuan[count2 - 1].Answer;
                                        }

                                        count2++;
                                    }
                        }
                        if (duoxuan.Count > 0)
                        {
                            wordDoc.Paragraphs.Last.Range.Text = "多选题\n";
                            wordApp.Selection.EndKey(ref unite, ref nothing);
                            var tableRow3 = (k / 15 + 1) * 2;
                            var tableColumn3 = k <= 15 ? k : 15;
                            //定义一个Word中的表格对象
                            var table3 = wordDoc.Tables.Add(wordApp.Selection.Range,
                                tableRow3, tableColumn3, ref nothing, ref nothing); //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
                            table3.Borders.Enable = 1; //这个值可以设置得很大，例如5、13等等
                            table3.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                            var count3 = 1;
                            for (var w = 1; w < tableRow3; w++)
                                if (w % 2 != 0)
                                    for (var e = 1; e <= tableColumn3; e++)
                                    {
                                        if (count3 < k)
                                        {
                                            table3.Cell(w, e).Range.Text = "" + count3; //填充每列的标题
                                            table3.Cell(w + 1, e).Range.Text = duoxuan[count3 - 1].Answer;
                                        }
                                        count3++;
                                    }
                        }
                        if (anli.Count > 0)
                        {
                            wordDoc.Paragraphs.Last.Range.Text = "案例题\n";
                            wordApp.Selection.EndKey(ref unite, ref nothing);
                            var tableRow4 = (m / 15 + 1) * 2;
                            var tableColumn4 = m <= 15 ? m : 15;
                            //定义一个Word中的表格对象
                            var table4 = wordDoc.Tables.Add(wordApp.Selection.Range,
                                tableRow4, tableColumn4, ref nothing, ref nothing); //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
                            table4.Borders.Enable = 1; //这个值可以设置得很大，例如5、13等等
                            table4.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                            var count4 = 1;
                            for (var w = 1; w < tableRow4; w++)
                                if (w % 2 != 0)
                                    for (var e = 1; e <= tableColumn4; e++)
                                    {
                                        if (count4 < m)
                                        {
                                            table4.Cell(w, e).Range.Text = "" + count4; //填充每列的标题
                                            table4.Cell(w + 1, e).Range.Text = anli[count4 - 1].Answer;
                                        }

                                        count4++;
                                    }
                        }
                    }
                    //为当前页添加页码
                    var pns = wordApp.Selection.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterEvenPages]
                        .PageNumbers; //获取当前页的号码
                    pns.NumberStyle = WdPageNumberStyle.wdPageNumberStyleNumberInDash; //设置页码的风格，是Dash形还是圆形的
                    pns.HeadingLevelForChapter = 0;
                    pns.IncludeChapterNumber = false;
                    pns.RestartNumberingAtSection = false;
                    pns.StartingNumber = 0; //开始页页码？
                    object pagenmbetal = WdPageNumberAlignment.wdAlignPageNumberCenter; //将号码设置在中间
                    object first = true;
                    wordApp.Selection.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers
                        .Add(ref pagenmbetal, ref first);
                    //WordDoc.Paragraphs.Last.Range.Text = "文档创建时间：" + DateTime.Now.ToString();//“落款”
                    test.Title += ".doc";
                    object filename = Server.MapPath("~/Uploads/") + test.Title; //文件保存路径
                    //文件保存
                    wordDoc.SaveAs(ref filename, ref nothing, ref nothing, ref nothing, ref nothing, ref nothing, ref nothing,
                        ref nothing, ref nothing, ref nothing, ref nothing, ref nothing, ref nothing, ref nothing, ref nothing,
                        ref nothing);
                    wordDoc.Close(ref nothing, ref nothing, ref nothing);
                    wordApp.Quit(ref nothing, ref nothing, ref nothing);
                    return Json(new { status = "success", title = test.Title }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = "Error:" + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error3" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportExamForPerson()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var userid = Convert.ToInt32(Request.Form["uid"]);
                var testid = Convert.ToInt32(Request.Form["tid"]);
                var ks = Convert.ToString(Request.Form["ks"]);
                var name = Convert.ToString(Request.Form["name"]);
                var score = Convert.ToInt32(Request.Form["score"]);
                var dict = TeachingModel.GetUserAnser(testid, userid);
                //var paperid = TeachingModel.GetPaperId(testid);
                var result = TeachingModel.GetExamTimu(user.HospitalId, testid);
                var data = (List<PaperTimu>)result.Data;
                var msg = TeachingModel.GetExam(user.HospitalId, testid);
                var test = (Test)msg.Data;
                try
                {
                    object nothing = Missing.Value;
                    //创建Word文档
                    Application wordApp = new ApplicationClass();
                    var wordDoc = wordApp.Documents.Add(ref nothing, ref nothing, ref nothing, ref nothing);
                    //添加页眉
                    wordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;
                    wordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;
                    wordApp.ActiveWindow.ActivePane.Selection.InsertAfter(
                        user.HospitalName + "       " + test.Begintime);
                    wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight; //设置右对齐
                    wordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument; //跳出页眉设置

                    wordApp.Selection.ParagraphFormat.LineSpacing = 15f; //设置文档的行间距
                    wordDoc.Paragraphs.Last.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    wordDoc.Paragraphs.Last.Range.Font.Size = 15;
                    wordDoc.Paragraphs.Last.Range.Font.Bold = 1;
                    wordDoc.Paragraphs.Last.Range.Text = test.Title + "\n";
                    wordDoc.Paragraphs.Last.Range.Font.Bold = 0;
                    wordDoc.Paragraphs.Last.Range.Font.Size = 11;
                    wordDoc.Paragraphs.Last.Range.Text = "科室：" + ks + "      姓名：" + name + "     得分：" + score + "\n";
                    wordDoc.Paragraphs.Last.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    if (data != null)
                    {
                        var i = 1;
                        var j = 1;
                        var k = 1;
                        var m = 1;
                        var danxuan = new List<PaperTimu>();
                        var duoxuan = new List<PaperTimu>();
                        var panduan = new List<PaperTimu>();
                        var anli = new List<PaperTimu>();
                        foreach (var timu in data)
                            if (timu.ExerciseType == 1 && string.IsNullOrEmpty(timu.Anli))
                                danxuan.Add(timu);
                            else if (timu.ExerciseType == 2 && string.IsNullOrEmpty(timu.Anli))
                                duoxuan.Add(timu);
                            else if (timu.ExerciseType == 3 && string.IsNullOrEmpty(timu.Anli))
                                panduan.Add(timu);
                            else
                                anli.Add(timu);
                        if (panduan.Count > 0)
                        {
                            wordDoc.Paragraphs.Last.Range.Text =
                                "一、判断题： （每题" + panduan[0].Cent + "分   共" + panduan[0].Cent * panduan.Count + "分）" +
                                "\n";
                            foreach (var timu in panduan)
                            {
                                wordDoc.Paragraphs.Last.Range.Text = "【" + timu.Answer + "】" + i + "、" + timu.Question + "(" + (dict != null && dict.Keys.Contains(timu.Tid) ? dict[timu.Tid] : "") + ")" + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = "A." + timu.ItemA + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = "B." + timu.ItemB + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = timu.ItemC + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = timu.ItemD + "\n";
                                i++;
                            }
                        }
                        wordDoc.Paragraphs.Last.Range.Text = "\n";
                        if (danxuan.Count > 0)
                        {
                            if (panduan.Count > 0)
                                wordDoc.Paragraphs.Last.Range.Text =
                                    "二、单选题： （每题" + danxuan[0].Cent + "分   共" + danxuan[0].Cent * danxuan.Count + "分）" +
                                    "\n";
                            else
                                wordDoc.Paragraphs.Last.Range.Text =
                                    "一、单选题： （每题" + danxuan[0].Cent + "分   共" + danxuan[0].Cent * danxuan.Count + "分）" +
                                    "\n";
                            foreach (var timu in danxuan)
                            {
                                wordDoc.Paragraphs.Last.Range.Text = "【" + timu.Answer + "】" + j + "、" + timu.Question + "(" + (dict != null && dict.Keys.Contains(timu.Tid) ? dict[timu.Tid] : "") + ")" + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = "A." + timu.ItemA + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = "B." + timu.ItemB + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemC))
                                    wordDoc.Paragraphs.Last.Range.Text = "C." + timu.ItemC + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemD))
                                    wordDoc.Paragraphs.Last.Range.Text = "D." + timu.ItemD + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemE))
                                    wordDoc.Paragraphs.Last.Range.Text = "E." + timu.ItemE + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemF))
                                    wordDoc.Paragraphs.Last.Range.Text = "F." + timu.ItemF + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemG))
                                    wordDoc.Paragraphs.Last.Range.Text = "G." + timu.ItemG + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemH))
                                    wordDoc.Paragraphs.Last.Range.Text = "H." + timu.ItemH + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemI))
                                    wordDoc.Paragraphs.Last.Range.Text = "I." + timu.ItemI + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemJ))
                                    wordDoc.Paragraphs.Last.Range.Text = "J." + timu.ItemJ + "\n";
                                j++;
                            }
                        }

                        wordDoc.Paragraphs.Last.Range.Text = "\n";

                        if (duoxuan.Count > 0)
                        {
                            if (panduan.Count > 0)
                            {
                                if (danxuan.Count > 0)
                                    wordDoc.Paragraphs.Last.Range.Text =
                                        "三、多选题： （每题" + duoxuan[0].Cent + "分   共" + duoxuan[0].Cent * duoxuan.Count +
                                        "分）" + "\n";
                                else
                                    wordDoc.Paragraphs.Last.Range.Text =
                                        "二、多选题： （每题" + duoxuan[0].Cent + "分   共" + duoxuan[0].Cent * duoxuan.Count +
                                        "分）" + "\n";
                            }
                            else
                            {
                                if (danxuan.Count > 0)
                                    wordDoc.Paragraphs.Last.Range.Text =
                                        "二、多选题： （每题" + duoxuan[0].Cent + "分   共" + duoxuan[0].Cent * duoxuan.Count +
                                        "分）" + "\n";
                                else
                                    wordDoc.Paragraphs.Last.Range.Text =
                                        "一、多选题： （每题" + duoxuan[0].Cent + "分   共" + duoxuan[0].Cent * duoxuan.Count +
                                        "分）" + "\n";
                            }
                            foreach (var timu in duoxuan)
                            {
                                wordDoc.Paragraphs.Last.Range.Text = "【" + timu.Answer + "】" + k + "、" + timu.Question + "(" + (dict != null && dict.Keys.Contains(timu.Tid) ? dict[timu.Tid] : "") + ")" + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = "A." + timu.ItemA + "\n";
                                wordDoc.Paragraphs.Last.Range.Text = "B." + timu.ItemB + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemC))
                                    wordDoc.Paragraphs.Last.Range.Text = "C." + timu.ItemC + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemD))
                                    wordDoc.Paragraphs.Last.Range.Text = "D." + timu.ItemD + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemE))
                                    wordDoc.Paragraphs.Last.Range.Text = "E." + timu.ItemE + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemF))
                                    wordDoc.Paragraphs.Last.Range.Text = "F." + timu.ItemF + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemG))
                                    wordDoc.Paragraphs.Last.Range.Text = "G." + timu.ItemG + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemH))
                                    wordDoc.Paragraphs.Last.Range.Text = "H." + timu.ItemH + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemI))
                                    wordDoc.Paragraphs.Last.Range.Text = "I." + timu.ItemI + "\n";
                                if (!string.IsNullOrEmpty(timu.ItemJ))
                                    wordDoc.Paragraphs.Last.Range.Text = "J." + timu.ItemJ + "\n";
                                k++;
                            }
                        }

                        wordDoc.Paragraphs.Last.Range.Text = "\n";

                        if (anli.Count > 0)
                        {
                            if (duoxuan.Count > 0)
                            {
                                if (panduan.Count > 0)
                                {
                                    if (danxuan.Count > 0)
                                        wordDoc.Paragraphs.Last.Range.Text = "四、案例题：" + "\n";
                                    else
                                        wordDoc.Paragraphs.Last.Range.Text = "三、案例题：" + "\n";
                                }
                                else
                                {
                                    if (danxuan.Count > 0)
                                        wordDoc.Paragraphs.Last.Range.Text = "三、案例题：" + "\n";
                                    else
                                        wordDoc.Paragraphs.Last.Range.Text = "二、案例题：" + "\n";
                                }
                            }
                            else
                            {
                                if (panduan.Count > 0)
                                {
                                    if (danxuan.Count > 0)
                                        wordDoc.Paragraphs.Last.Range.Text = "三、案例题：" + "\n";
                                    else
                                        wordDoc.Paragraphs.Last.Range.Text = "二、案例题：" + "\n";
                                }
                                else
                                {
                                    if (danxuan.Count > 0)
                                        wordDoc.Paragraphs.Last.Range.Text = "二、案例题：" + "\n";
                                    else
                                        wordDoc.Paragraphs.Last.Range.Text = "一、案例题：" + "\n";
                                }
                            }
                            var anlistr = new List<string>();
                            foreach (var timu in anli)
                            {
                                if (anlistr.Count > 0 && anlistr.Contains(timu.Anli))
                                    continue;
                                anlistr.Add(timu.Anli);
                            }
                            foreach (var t in anlistr)
                            {
                                wordDoc.Paragraphs.Last.Range.Text = "案例：  " + t + "\n";
                                foreach (var timu in anli)
                                    if (timu.Anli.Equals(t))
                                    {
                                        if (timu.ExerciseType == 1)
                                            wordDoc.Paragraphs.Last.Range.Text =
                                                "【" + timu.Answer + "】" + m + "、【单选题】" + timu.Question + "(" + (dict != null && dict.Keys.Contains(timu.Tid) ? dict[timu.Tid] : "") + ")" + "\n";
                                        if (timu.ExerciseType == 2)
                                            wordDoc.Paragraphs.Last.Range.Text =
                                                "【" + timu.Answer + "】" + m + "、【多选题】" + timu.Question + "(" + (dict != null && dict.Keys.Contains(timu.Tid) ? dict[timu.Tid] : "") + ")" + "\n";
                                        if (timu.ExerciseType == 3)
                                            wordDoc.Paragraphs.Last.Range.Text =
                                                "【" + timu.Answer + "】" + m + "、【判断题】" + timu.Question + "(" + (dict != null && dict.Keys.Contains(timu.Tid) ? dict[timu.Tid] : "") + ")" + "\n";
                                        wordDoc.Paragraphs.Last.Range.Text = "A." + timu.ItemA + "\n";
                                        wordDoc.Paragraphs.Last.Range.Text = "B." + timu.ItemB + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemC))
                                            wordDoc.Paragraphs.Last.Range.Text = "C." + timu.ItemC + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemD))
                                            wordDoc.Paragraphs.Last.Range.Text = "D." + timu.ItemD + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemE))
                                            wordDoc.Paragraphs.Last.Range.Text = "E." + timu.ItemE + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemF))
                                            wordDoc.Paragraphs.Last.Range.Text = "F." + timu.ItemF + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemG))
                                            wordDoc.Paragraphs.Last.Range.Text = "G." + timu.ItemG + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemH))
                                            wordDoc.Paragraphs.Last.Range.Text = "H." + timu.ItemH + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemI))
                                            wordDoc.Paragraphs.Last.Range.Text = "I." + timu.ItemI + "\n";
                                        if (!string.IsNullOrEmpty(timu.ItemJ))
                                            wordDoc.Paragraphs.Last.Range.Text = "J." + timu.ItemJ + "\n";
                                        m++;
                                    }
                            }
                        }
                        //wordDoc.Paragraphs.Last.Range.InsertBreak();
                    }
                    //为当前页添加页码
                    var pns = wordApp.Selection.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterEvenPages]
                        .PageNumbers; //获取当前页的号码
                    pns.NumberStyle = WdPageNumberStyle.wdPageNumberStyleNumberInDash; //设置页码的风格，是Dash形还是圆形的
                    pns.HeadingLevelForChapter = 0;
                    pns.IncludeChapterNumber = false;
                    pns.RestartNumberingAtSection = false;
                    pns.StartingNumber = 0; //开始页页码？
                    object pagenmbetal = WdPageNumberAlignment.wdAlignPageNumberCenter; //将号码设置在中间
                    object first = true;
                    wordApp.Selection.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers
                        .Add(ref pagenmbetal, ref first);
                    //WordDoc.Paragraphs.Last.Range.Text = "文档创建时间：" + DateTime.Now.ToString();//“落款”
                    test.Title += ".doc";
                    object filename = Server.MapPath("~/Uploads/") + test.Title; //文件保存路径
                    //文件保存
                    wordDoc.SaveAs(ref filename, ref nothing, ref nothing, ref nothing, ref nothing, ref nothing,
                        ref nothing,
                        ref nothing, ref nothing, ref nothing, ref nothing, ref nothing, ref nothing, ref nothing,
                        ref nothing,
                        ref nothing);
                    wordDoc.Close(ref nothing, ref nothing, ref nothing);
                    wordApp.Quit(ref nothing, ref nothing, ref nothing);
                    return Json(new { status = "success", title = test.Title }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = "Error:" + ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error3" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportExamWord()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToString(Request.Form["id"]);
                var grade = Convert.ToInt32(Request.Form["grade"]);
                var month = Convert.ToString(Request.Form["month"]);
                var deptcode = Convert.ToString(Request.Form["deptcode"]);
                var loginid = Convert.ToString(Request.Form["loginId"]);
                var name = Convert.ToString(Request.Form["name"]);
                var result = TeachingModel.GetExamInfoDetail(grade, month, id, deptcode, loginid, name);
                var data = (List<ExamInfoDetail>)result.Data;
                if (data != null && data.Count > 0)
                {
                    var fileName = user.HospitalName + "_" + data[0].Content + ".doc";
                    ExcelHelper.ExportDataToWord(data, Server.MapPath("~/Uploads/") + fileName);
                    return Json(new { status = "success", title = fileName }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error3" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportExamXls()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToString(Request.Form["id"]);
                var grade = Convert.ToInt32(Request.Form["grade"]);
                var month = Convert.ToString(Request.Form["month"]);
                var deptcode = Convert.ToString(Request.Form["deptcode"]);
                var loginid = Convert.ToString(Request.Form["loginId"]);
                var name = Convert.ToString(Request.Form["name"]);

                var result = TeachingModel.GetExamInfoDetail(grade, month, id, deptcode, loginid, name);
                var data = (List<ExamInfoDetail>)result.Data;
                if (data != null && data.Count > 0)
                {
                    var fileName = user.HospitalName + "_" + data[0].Content + ".xls";
                    ExcelHelper.ExportDataToExcel(data, "考试情况", Server.MapPath("~/Uploads/" + fileName));
                    return Json(new { status = "success", title = fileName }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "Error2" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Error3" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 实操评分

        public ActionResult OperationT()
        {
            return View();
        }

        public ActionResult Operation()
        {
            return View();
        }

        #endregion
    }
}