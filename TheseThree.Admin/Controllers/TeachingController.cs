using System;
using System.Collections.Generic;
using System.IO;
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
                var paper = TeachingModel.GetPaper(user.HospitalId);
                ViewBag.Paper = paper;
                var models = new OrganizationModel().GetCommonAttr(user.HospitalId);
                return View(models);
            }
            return null;
        }
        public JsonResult GetKsUser(int limit, int offset, int examid, string name, string loginId, string deptname,
            string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname, string xzcode, string xzname)
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
        public JsonResult GetTrain(int limit, int offset, string name, string orgname, int orgtype)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTrain(name, orgname, orgtype, user.HospitalId);
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
                string ids = Convert.ToString(Request.Form["id"]);
                int trainid = Convert.ToInt32(Request.Form["trainid"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var result = TeachingModel.UpdateTrainUser(ids, trainid, user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelTrainUser()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                int examid = Convert.ToInt32(Request.Form["examid"]);
                var result = TeachingModel.DeleteTrainUser(id, examid);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTrainDetail(int limit,int offset,int examid)
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
        public JsonResult DelTrainDetail()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                int examid = Convert.ToInt32(Request.Form["examid"]);
                var result = TeachingModel.DelTrainDetail(id, examid); 
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveTrain()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string zhuti = Convert.ToString(Request.Form["zhuti"]);
                int trainid = Convert.ToInt32(Request.Form["trainid"]);
                string org = Convert.ToString(Request.Form["org"]);
                string time = Convert.ToString(Request.Form["time"]);
                string teacher = Convert.ToString(Request.Form["teacher"]);
                string address = Convert.ToString(Request.Form["address"]);
                int score = Convert.ToInt32(Request.Form["score"]);
                int type = Convert.ToInt32(Request.Form["type"]);
                var result = TeachingModel.UpdateTrain(trainid, zhuti, address, org, time, teacher,score,type, user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveTrainDetail()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string title = Convert.ToString(Request.Form["title"]);
                int trainid = Convert.ToInt32(Request.Form["trainid"]);
                string tec = Convert.ToString(Request.Form["tec"]);
                var result = TeachingModel.UpdateTrainDetail(trainid, title, tec);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelTrainFj()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                int type = Convert.ToInt32(Request.Form["type"]);
                int examid = Convert.ToInt32(Request.Form["examid"]);
                var result = TeachingModel.DelTrainFj(id,type, examid);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
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

        public JsonResult UploadFuJian(HttpPostedFileBase fileData, int trainid,string kjname,int type)
        {

            var user = GetCurrentUser();
            if (user != null)
            {
                if (fileData != null)
                {
                    try
                    {

                        //string filePath = @"E:\jboss-as-7.1.1.Final\welcome-content\kj\" + user.HospitalId + "\\";
                        //if (!Directory.Exists(filePath))
                        //{
                        //    Directory.CreateDirectory(filePath);
                        //}
                        //var fileName = Path.GetFileName(fileData.FileName);
                        //var fileExtension = Path.GetExtension(fileName);
                        //var saveName = Guid.NewGuid() + fileExtension;
                        //fileData.SaveAs(filePath + saveName);
                        //TeachingModel.AddTrainFj(trainid, kjname, saveName, type);
                        //return Json(new { Success = false });
                    

                        var filePath = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        var fileName = Path.GetFileName(fileData.FileName);
                        var fileExtension = Path.GetExtension(fileName);
                        var saveName = Guid.NewGuid() + fileExtension;
                        fileData.SaveAs(filePath + saveName);
                        TeachingModel.AddTrainFj(trainid, kjname, saveName, type);
                        return Json(new { Success = false });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, Message = "用户身份失效，请重新登陆。" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 题库管理
        [Authentication]
        public ActionResult Subject()
        {
            return View();
        }

        [Authentication]
        public JsonResult GetTiKu(int limit, int offset, string name)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTiKu(user.HospitalId, name);
                var data = (List<TiKu>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiMu(int limit, int offset, int section, string name,string type, string labelname, string labelcode)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTiMu(user.HospitalId, name,type, labelname, labelcode, section);
                var data = (List<TiMu>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "[]" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiMus(int limit, int offset,int paperid, int section, string name, string labelname, string labelcode, int exerciseType)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetTiMu(paperid,user.HospitalId, name, labelname, labelcode, section, exerciseType);
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
                int id = Convert.ToInt32(Request.Form["id"]);
                string name = Request.Form["name"];
                var result = TeachingModel.UpdateTiKu(id, name, user.HospitalId, user.UserName);
                if (result.Status != MessageType.Error)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        [Authentication]
        public JsonResult DelTiKu()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var result = TeachingModel.DeleteTk(ids, user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
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
                {
                    return null;
                }
                var list = TeachingModel.GetLabel(id, user.HospitalId);
                ViewBag.SectionId = id;
                return View(list);
            }
            return null;
        }
        [Authentication]
        public JsonResult UploadSubject(HttpPostedFileBase fileData, int sectionId)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                if (fileData != null)
                {
                    try
                    {
                        var filePath = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        var fileName = Path.GetFileName(fileData.FileName);
                        var fileExtension = Path.GetExtension(fileName);
                        var saveName = Guid.NewGuid() + fileExtension;
                        fileData.SaveAs(filePath + saveName);
                        var labels = TeachingModel.GetLabels(sectionId, user.HospitalId);
                        var list = ExcelHelper.ImportSubject(filePath + saveName, user.HospitalId, sectionId, labels);
                        if (list == null)
                        {
                            return Json(new { Success = true, Message = "上传成功" });
                        }
                        return Json(new { Success = false, Message = list });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
                    }
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
                var question = Convert.ToString(Request.Form["question"]);
                var difficulty = Convert.ToSingle(Request.Form["difficulty"]);
                var label = Convert.ToString(Request.Form["label"]);
                var type = Convert.ToString(Request.Form["type"]);
                var itema = Convert.ToString(Request.Form["itema"]);
                var itema_c = Convert.ToInt32(Request.Form["itema_c"]);
                var itemb = Convert.ToString(Request.Form["itemb"]);
                var itemb_c = Convert.ToInt32(Request.Form["itemb_c"]);
                var itemc = Convert.ToString(Request.Form["itemc"]);
                var itemc_c = Convert.ToInt32(Request.Form["itemc_c"]);
                var itemd = Convert.ToString(Request.Form["itemd"]);
                var itemd_c = Convert.ToInt32(Request.Form["itemd_c"]);
                var iteme = Convert.ToString(Request.Form["iteme"]);
                var iteme_c = Convert.ToInt32(Request.Form["iteme_c"]);
                var itemf = Convert.ToString(Request.Form["itemf"]);
                var itemf_c = Convert.ToInt32(Request.Form["itemf_c"]);
                var itemg = Convert.ToString(Request.Form["itemg"]);
                var itemg_c = Convert.ToInt32(Request.Form["itemg_c"]);
                var itemh = Convert.ToString(Request.Form["itemh"]);
                var itemh_c = Convert.ToInt32(Request.Form["itemh_c"]);
                var itemi = Convert.ToString(Request.Form["itemi"]);
                var itemi_c = Convert.ToInt32(Request.Form["itemi_c"]);
                var itemj = Convert.ToString(Request.Form["itemj"]);
                var itemj_c = Convert.ToInt32(Request.Form["itemj_c"]);
                var result = TeachingModel.UpdateTiMu(id, anli, question, label,type, itema, itema_c, itemb, itemb_c, itemc, itemc_c, itemd, itemd_c, iteme, iteme_c, itemf, itemf_c, itemg, itemg_c, itemh, itemh_c, itemi, itemi_c, itemj, itemj_c, sectionId, difficulty, user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        [Authentication]
        public JsonResult DelTiMu()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                int sectionid = Convert.ToInt32(Request.Form["sectionId"]);
                var result = TeachingModel.DeleteTm(ids, sectionid);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
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
                var result = TeachingModel.GetPaper(user.HospitalId, name);
                var data = (List<Paper>)result.Data;
                if (data != null && data.Count > 0)
                    return Json(new { total = data.Count, rows = data.Skip(offset).Take(limit).ToList() },
                        JsonRequestBehavior.AllowGet);
            }
            return Json(new { total = 0, rows = "" }, JsonRequestBehavior.AllowGet);
        }
        [Authentication]
        public JsonResult GetPaperTimu( int paperid,int exerciseType)
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
                return Json(new {status = "success", data = result}, JsonRequestBehavior.AllowGet);
            }
            return Json(new {status = "Error"}, JsonRequestBehavior.AllowGet);
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
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        [Authentication]
        public JsonResult AddPaperQuestion()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var exerciseType = Convert.ToInt32(Request.Form["exerciseType"]);
                var paperid = Convert.ToInt32(Request.Form["paperid"]);
                var cent = Convert.ToInt32(Request.Form["cent"]); 
                var result = TeachingModel.UpdatePaperQuestion(ids, exerciseType, paperid, user.HospitalId,cent);
                if (result.Status != MessageType.Error)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        [Authentication]
        public JsonResult DelPaperQuestion()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                int id = Convert.ToInt32(Request.Form["id"]);
                var exerciseType = Convert.ToInt32(Request.Form["exerciseType"]);
                var paperid = Convert.ToInt32(Request.Form["paperid"]);
                var result = TeachingModel.DelPaperQuestion(id, paperid, exerciseType, user.HospitalId);
                if (result)
                {
                    return Json(new {status = "success"}, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new {status = "Error"}, JsonRequestBehavior.AllowGet);
        }
        [Authentication]
        public JsonResult DelPaper()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var result = TeachingModel.DeletePaper(ids, user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
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
                var paper = (Paper) msg.Data;
                ViewBag.PaperId = paper.Id;
                ViewBag.PaperName = paper.Name;
                ViewBag.TotalMini = paper.Duration;
                var tiku = TeachingModel.GetTiKu(user.HospitalId, "");
                if (tiku != null)
                {
                    var tk = (List<TiKu>)tiku.Data;
                    if (tk != null && tk.Count > 0)
                    {
                        ViewBag.Tiku = tk;
                    }
                }
                return View();
            }
            return null;
        }
        public JsonResult GetLabels()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var id = Convert.ToInt32(Request.Form["sectionId"]); 
                var result = TeachingModel.GetLabels(id, user.HospitalId);
                return Json(new { status = "success",data=result }, JsonRequestBehavior.AllowGet);
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
                var result = TeachingModel.SavePaper(paperId,papername,totalMini, user.HospitalId);
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
        public JsonResult GetExam(int limit, int offset, string name,string startTime,string endTime)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetExam(user.HospitalId, name, startTime,endTime);
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
                ViewBag.Jigescore =test.Jigescore;
                ViewBag.PaperId = test.PaperId;
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
                string ids = Convert.ToString(Request.Form["id"]);
                int examid= Convert.ToInt32(Request.Form["examid"]);
                int usertype = Convert.ToInt32(Request.Form["usertype"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var result = TeachingModel.UpdateExamUser(ids, examid, usertype, user.HospitalId);
                if (result.Status != MessageType.Error)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserForExam(int limit, int offset,int examId)
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
                int id = Convert.ToInt32(Request.Form["id"]);
                int userType = Convert.ToInt32(Request.Form["userType"]);
                int examid = Convert.ToInt32(Request.Form["examid"]);
                var result = TeachingModel.DeleteExamUser(id, userType,examid);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetKsUserForExam(int limit, int offset, int examid, string name, string loginId, string deptname,
    string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname, string xzcode, string xzname)
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                var result = TeachingModel.GetKsUserForExam(examid, user.HospitalId, name, loginId, deptname, deptcode, gwcode,
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
                string examname = Convert.ToString(Request.Form["examname"]);
                int examid = Convert.ToInt32(Request.Form["examid"]);
                string paper = Convert.ToString(Request.Form["paper"]);
                string startTime = Convert.ToString(Request.Form["startTime"]);
                string endTime = Convert.ToString(Request.Form["endTime"]);
                string jigescroe = Convert.ToString(Request.Form["jigescroe"]);
                string address = Convert.ToString(Request.Form["address"]);
                var result = TeachingModel.UpdateExam(examid,examname,address,startTime,endTime,paper,jigescroe,user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelExam()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var result = TeachingModel.DelExam(ids, user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelTrain()
        {
            var user = GetCurrentUser();
            if (user != null)
            {
                string ids = Convert.ToString(Request.Form["id"]);
                if (ids.StartsWith(","))
                {
                    ids = ids.Substring(1);
                }
                var result = TeachingModel.DelTrain(ids, user.HospitalId);
                if (result)
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
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