﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TheseThree.Admin.DataAccess;
using TheseThree.Admin.Models.Entities;
using TheseThree.Admin.Models.ViewModels;

namespace TheseThree.Admin.Models
{
    public class TeachingModel
    {
        public static Message UpdateTiKu(int id, string name, int userHospitalId, string username)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "未更新任何数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql;
                    if (id > 0)
                    {
                        sql = string.Format("update section set name='{0}' where hospitalId={1} and id={2}", name,
                            userHospitalId, id);
                    }
                    else
                    {
                        sql =
                            string.Format(
                                "insert into section(name,count,remark,hospitalId) values('{0}',0,'{1}',{2});", name,
                                username, userHospitalId);
                    }
                    var result = dao.ExecuteCommand(sql);
                    if (result > 0)
                    {
                        message.Status = MessageType.Success;
                        message.Msg = "成功";
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }
        public static Message UpdateTiKuCount(int sectionId)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "未更新任何数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql =
                        string.Format(
                            "update section set count=(SELECT count(1) as qty FROM `timu` where section={0}) where section={0}",
                            sectionId);
                    var result = dao.ExecuteCommand(sql);
                    if (result > 0)
                    {
                        message.Status = MessageType.Success;
                        message.Msg = "成功";
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }
        public static Message GetTiKu(int hospitalid, string name)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select * from section where Hospitalid=" + hospitalid;
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += " and  name like '%" + name + "%'  ";
                    }
                    var result =
                        dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<TiKu> tiKus = new List<TiKu>();
                        foreach (DataRow row in result.Rows)
                        {
                            var tiku = new TiKu
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Name = Convert.ToString(row["name"]),
                                Count = Convert.ToInt32(row["count"]),
                                HospitalId = Convert.ToInt32(row["hospitalid"]),
                                Remark = Convert.ToString(row["remark"])
                            };
                            tiKus.Add(tiku);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = tiKus;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }
        public static bool DeleteTk(string ids, int userHospitalId)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "delete from SECTION where id in (" + ids + ")  and hospitaliD=" + userHospitalId + ";";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }
        public static Message AddTimu(string sql)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "未更新任何数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var result = dao.ExecuteCommand(sql);
                    if (result > 0)
                    {
                        message.Status = MessageType.Success;
                        message.Msg = "成功";
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }
            return message;
        }
        public static List<string> GetLabels(int sectionId, int hospitalId)
        {
            List<string> result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "select content from label where sectionId =" + sectionId + "  and hospitaliD=" +
                                 hospitalId + ";";
                    result = dao.GetList<string>(sql);
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }
        public static List<string> GetLabels(int hospitalId)
        {
            List<string> result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "select content from label where  hospitaliD=" + hospitalId + ";";
                    result = dao.GetList<string>(sql);
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }
        public static List<CommonEntityViewModel> GetLabel(int sectionId, int hospitalId)
        {
            var result = new List<CommonEntityViewModel>();
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "select id,content from label where sectionId =" + sectionId + "  and hospitaliD=" +
                                 hospitalId + ";";
                    var data = dao.GetDataTable(sql);
                    if (data != null && data.Rows.Count > 0)
                    {
                        result.AddRange(from DataRow row in data.Rows
                                        select new CommonEntityViewModel
                                        {
                                            Value = Convert.ToInt32(row["id"]),
                                            Name = Convert.ToString(row["content"])
                                        });
                    }
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }
        public static Message GetTiMu(int hospitalId, string name, string type,string labelname, string labelcode, int sectionId)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select * from timu where section=" + sectionId + " ";
                    if (!string.IsNullOrEmpty(type) && type != "-1")
                    {
                        sql += " and exerciseType ="+type;
                    }
                        if (!string.IsNullOrEmpty(labelname) && labelcode != "-1")
                    {
                        sql += " and label like '%" + labelname + "%' ";
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += "  and question like '%" + name + "%'  ";
                    }
                    var result =
                        dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<TiMu> tiMus = new List<TiMu>();
                        foreach (DataRow row in result.Rows)
                        {
                            var timu = new TiMu { Id = Convert.ToInt32(row["id"]) };
                            if (DBNull.Value != row["answer"])
                            {
                                timu.Answer = Convert.ToString(row["answer"]);
                            }
                            if (DBNull.Value != row["exerciseType"])
                            {
                                timu.ExerciseType = Convert.ToInt32(row["exerciseType"]);
                            }
                            if (DBNull.Value != row["itemA"])
                            {
                                timu.ItemA = Convert.ToString(row["itemA"]);
                            }
                            if (DBNull.Value != row["itemB"])
                            {
                                timu.ItemB = Convert.ToString(row["itemB"]);
                            }
                            if (DBNull.Value != row["itemC"])
                            {
                                timu.ItemC = Convert.ToString(row["itemC"]);
                            }
                            if (DBNull.Value != row["itemD"])
                            {
                                timu.ItemD = Convert.ToString(row["itemD"]);
                            }
                            if (DBNull.Value != row["itemE"])
                            {
                                timu.ItemE = Convert.ToString(row["itemE"]);
                            }
                            if (DBNull.Value != row["itemF"])
                            {
                                timu.ItemF = Convert.ToString(row["itemF"]);
                            }
                            if (DBNull.Value != row["itemG"])
                            {
                                timu.ItemG = Convert.ToString(row["itemG"]);
                            }
                            if (DBNull.Value != row["itemH"])
                            {
                                timu.ItemH = Convert.ToString(row["itemH"]);
                            }
                            if (DBNull.Value != row["itemI"])
                            {
                                timu.ItemI = Convert.ToString(row["itemI"]);
                            }
                            if (DBNull.Value != row["itemJ"])
                            {
                                timu.ItemJ = Convert.ToString(row["itemJ"]);
                            }
                            if (DBNull.Value != row["itemNum"])
                            {
                                timu.ItemNum = Convert.ToInt32(row["itemNum"]);
                            }
                            if (DBNull.Value != row["anli"])
                            {
                                timu.Anli = Convert.ToString(row["anli"]);
                            }
                            if (DBNull.Value != row["question"])
                            {
                                timu.Question = Convert.ToString(row["question"]);
                            }
                            if (DBNull.Value != row["remark"])
                            {
                                timu.Remark = Convert.ToString(row["remark"]);
                            }
                            if (DBNull.Value != row["section"])
                            {
                                timu.Section = Convert.ToInt32(row["section"]);
                            }
                            if (DBNull.Value != row["label"])
                            {
                                timu.Label = Convert.ToString(row["label"]);
                            }
                            if (DBNull.Value != row["difficulty"])
                            {
                                timu.Difficulty = Convert.ToSingle(row["difficulty"]);
                            }
                            tiMus.Add(timu);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = tiMus;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }
        public static Message GetTiMu(int paperid, int hospitalId, string name, string labelname, string labelcode,
            int sectionId, int exerciseType)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select * from timu where id not in (select tid from paper_question where paperid=" +
                              paperid + ") and  section=" + sectionId + " and exerciseType=" + exerciseType + " ";
                    if (!string.IsNullOrEmpty(labelname) && labelcode != "-1")
                    {
                        sql += " and label like '%" + labelname + "%' ";
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += "  and question like '%" + name + "%'  ";
                    }
                    var result =
                        dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<TiMu> tiMus = new List<TiMu>();
                        foreach (DataRow row in result.Rows)
                        {
                            var timu = new TiMu { Id = Convert.ToInt32(row["id"]) };
                            if (DBNull.Value != row["answer"])
                            {
                                timu.Answer = Convert.ToString(row["answer"]);
                            }
                            if (DBNull.Value != row["exerciseType"])
                            {
                                timu.ExerciseType = Convert.ToInt32(row["exerciseType"]);
                            }
                            if (DBNull.Value != row["itemA"])
                            {
                                timu.ItemA = Convert.ToString(row["itemA"]);
                            }
                            if (DBNull.Value != row["itemB"])
                            {
                                timu.ItemB = Convert.ToString(row["itemB"]);
                            }
                            if (DBNull.Value != row["itemC"])
                            {
                                timu.ItemC = Convert.ToString(row["itemC"]);
                            }
                            if (DBNull.Value != row["itemD"])
                            {
                                timu.ItemD = Convert.ToString(row["itemD"]);
                            }
                            if (DBNull.Value != row["itemE"])
                            {
                                timu.ItemE = Convert.ToString(row["itemE"]);
                            }
                            if (DBNull.Value != row["itemF"])
                            {
                                timu.ItemF = Convert.ToString(row["itemF"]);
                            }
                            if (DBNull.Value != row["itemG"])
                            {
                                timu.ItemG = Convert.ToString(row["itemG"]);
                            }
                            if (DBNull.Value != row["itemH"])
                            {
                                timu.ItemH = Convert.ToString(row["itemH"]);
                            }
                            if (DBNull.Value != row["itemI"])
                            {
                                timu.ItemI = Convert.ToString(row["itemI"]);
                            }
                            if (DBNull.Value != row["itemJ"])
                            {
                                timu.ItemJ = Convert.ToString(row["itemJ"]);
                            }
                            if (DBNull.Value != row["itemNum"])
                            {
                                timu.ItemNum = Convert.ToInt32(row["itemNum"]);
                            }
                            if (DBNull.Value != row["anli"])
                            {
                                timu.Anli = Convert.ToString(row["anli"]);
                            }
                            if (DBNull.Value != row["question"])
                            {
                                timu.Question = Convert.ToString(row["question"]);
                            }
                            if (DBNull.Value != row["remark"])
                            {
                                timu.Remark = Convert.ToString(row["remark"]);
                            }
                            if (DBNull.Value != row["section"])
                            {
                                timu.Section = Convert.ToInt32(row["section"]);
                            }
                            if (DBNull.Value != row["label"])
                            {
                                timu.Label = Convert.ToString(row["label"]);
                            }
                            if (DBNull.Value != row["difficulty"])
                            {
                                timu.Difficulty = Convert.ToSingle(row["difficulty"]);
                            }
                            tiMus.Add(timu);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = tiMus;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }
        public static Message GetPaperTimu(int hospitalId, int id, int exerciseType)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql =
                        "SELECT a.id,a.paperid,a.exerciseType,b.question,b.answer,b.itema,b.itemb,b.itemc,b.itemd,b.iteme,b.itemf,b.itemg,b.itemh,b.itemi,b.itemj,a.cent FROM `paper_question` a LEFT JOIN timu b ON a.tid=b.id WHERE a.paperid=" +
                        id + " and a.exerciseType=" + exerciseType + " AND a.hospitaliD=" + hospitalId + " ;";
                    var result =
                        dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<PaperTimu> tiMus = new List<PaperTimu>();
                        foreach (DataRow row in result.Rows)
                        {
                            var timu = new PaperTimu { Id = Convert.ToInt32(row["id"]) };
                            if (DBNull.Value != row["question"])
                            {
                                timu.Question = Convert.ToString(row["question"]);
                            }
                            if (DBNull.Value != row["ItemA"])
                            {
                                timu.ItemA = Convert.ToString(row["ItemA"]);
                            }
                            if (DBNull.Value != row["ItemB"])
                            {
                                timu.ItemB = Convert.ToString(row["ItemB"]);
                            }
                            if (DBNull.Value != row["ItemC"])
                            {
                                timu.ItemC = Convert.ToString(row["ItemC"]);
                            }
                            if (DBNull.Value != row["ItemD"])
                            {
                                timu.ItemD = Convert.ToString(row["ItemD"]);
                            }
                            if (DBNull.Value != row["ItemE"])
                            {
                                timu.ItemE = Convert.ToString(row["ItemE"]);
                            }
                            if (DBNull.Value != row["ItemF"])
                            {
                                timu.ItemF = Convert.ToString(row["ItemF"]);
                            }
                            if (DBNull.Value != row["ItemG"])
                            {
                                timu.ItemG = Convert.ToString(row["ItemG"]);
                            }
                            if (DBNull.Value != row["ItemH"])
                            {
                                timu.ItemH = Convert.ToString(row["ItemH"]);
                            }
                            if (DBNull.Value != row["ItemI"])
                            {
                                timu.ItemI = Convert.ToString(row["ItemI"]);
                            }
                            if (DBNull.Value != row["ItemJ"])
                            {
                                timu.ItemJ = Convert.ToString(row["ItemJ"]);
                            }
                            if (DBNull.Value != row["PaperId"])
                            {
                                timu.PaperId = Convert.ToInt32(row["PaperId"]);
                            }
                            if (DBNull.Value != row["exerciseType"])
                            {
                                timu.ExerciseType = Convert.ToInt32(row["exerciseType"]);
                            }
                            if (DBNull.Value != row["Cent"])
                            {
                                timu.Cent = Convert.ToInt32(row["Cent"]);
                            }
                            if (DBNull.Value != row["Answer"])
                            {
                                timu.Answer = Convert.ToString(row["Answer"]);
                            }
                            tiMus.Add(timu);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = tiMus;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }
        public static void UpdatePaperQuestion(int paperid, int hospitalId)
        {
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "update papers set Count=(SELECT count(1) FROM paper_question where paperid=" + paperid +
                              "),TotalCent=(SELECT SUM(CENT) FROM paper_question where paperid=" + paperid +
                              ") where Id=" + paperid + " and hospitalId=" + hospitalId + ";";
                    dao.ExecuteCommand(sql);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }
        public static Message UpdateTiMu(int id, string anli, string question, string label,string type, string itema, int itemaC,
            string itemb, int itembC, string itemc, int itemcC, string itemd, int itemdC, string iteme, int itemeC,
            string itemf, int itemfC, string itemg, int itemgC, string itemh, int itemhC, string itemi, int itemiC,
            string itemj, int itemjC, int sectionId, float difficulty, int hospitalId)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "未更新任何数据",
                Data = null
            };
            try
            {
                var itemNum = 0;
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string answer = "";
                    if (itemaC == 1)
                    {
                        answer += "A";
                    }
                    if (itembC == 1)
                    {
                        answer += "B";
                    }
                    if (itemcC == 1)
                    {
                        answer += "C";
                    }
                    if (itemdC == 1)
                    {
                        answer += "D";
                    }
                    if (itemeC == 1)
                    {
                        answer += "E";
                    }
                    if (itemfC == 1)
                    {
                        answer += "F";
                    }
                    if (itemgC == 1)
                    {
                        answer += "G";
                    }
                    if (itemhC == 1)
                    {
                        answer += "H";
                    }
                    if (itemiC == 1)
                    {
                        answer += "I";
                    }
                    if (itemjC == 1)
                    {
                        answer += "J";
                    }
                    
                    string sql;
                    if (id > 0)
                    {
                        sql =
                            string.Format(
                                "update timu set answer='{0}',exerciseType={1},itemA='{2}',itemB='{3}',itemC='{4}',itemD='{5}',itemE='{6}',itemF='{7}',itemG='{8}',itemH='{9}',itemI='{10}',itemJ='{11}',itemNum={12},anli='{13}',question='{14}',label='{15}',difficulty={16} where section={17} and id={18}",
                                answer, type, itema, itemb, itemc, itemd, iteme, itemf, itemg, itemh, itemi,
                                itemj, itemNum, anli,
                                question, label, difficulty, sectionId, id);
                    }
                    else
                    {
                        sql =
                            string.Format(
                                "insert into timu(answer,exerciseType,itemA,itemB,itemC,itemD,itemE,itemF,itemG,itemH,itemI,itemJ,itemNum,anli,question,section,label,difficulty) values('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},'{13}','{14}',{15},'{16}',{17});",
                                answer, type, itema, itemb, itemc, itemd, iteme, itemf, itemg, itemh, itemi,
                                itemj, itemNum, anli, question, sectionId, label, difficulty);
                        if (!string.IsNullOrEmpty(label))
                        {
                            if (label.Contains("，"))
                            {
                                label = label.Replace("，", ",");
                                var arr = label.Split(',');
                                foreach (var item in arr)
                                {
                                    sql +=
                                        string.Format(
                                            "insert into label(content,sectionId,hospitalId)values('{0}',{1},{2});",
                                            item, sectionId, hospitalId);
                                }
                            }
                        }

                        sql += "update section set count=(select count(1) from timu where section=" + sectionId +
                               ") where id=" + sectionId + " and hospitalid=" + hospitalId + ";";
                    }
                    var result = dao.ExecuteCommand(sql);
                    if (result > 0)
                    {
                        message.Status = MessageType.Success;
                        message.Msg = "成功";
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        /// <summary>
        /// 删除题目，删除标签未做。
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="sectionid"></param>
        /// <returns></returns>
        public static bool DeleteTm(string ids, int sectionid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "delete from timu where id in (" + ids + ")  and section=" + sectionid + ";";
                    sql += "update section set count=(select count(1) from timu where section=" + sectionid +
                           ") where id=" + sectionid + ";";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static Message UpdatePaper(int id, string name, int deptCode, string deptname, int hospitalId)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "未更新任何数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql;
                    if (id > 0)
                    {
                        sql =
                            string.Format(
                                "update Papers set name='{0}',deptcode={1},deptname='{2}' where id={3} and hospitalid={4}",
                                name, deptCode, deptname, id, hospitalId);
                    }
                    else
                    {
                        sql =
                            string.Format(
                                "insert into Papers(name,count,totalcent,deptcode,deptname,hospitalId) values('{0}',0,0,{1},'{2}',{3});",
                                name,
                                deptCode, deptname, hospitalId);
                    }
                    var result = dao.ExecuteCommand(sql);
                    if (result > 0)
                    {
                        message.Status = MessageType.Success;
                        message.Msg = "成功";
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static Message UpdatePaperQuestion(string ids, int exerciseType, int paperid, int hospitalId, int cent)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "未更新任何数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "";
                    if (ids.Contains(","))
                    {
                        var arr = ids.Split(',');
                        foreach (var item in arr)
                        {
                            sql +=
                                string.Format(
                                    "insert into paper_question(paperid,tid,exerciseType,hospitaliD,cent) values({0},{1},{2},{3},{4});",
                                    paperid, item, exerciseType, hospitalId, cent);
                        }
                    }
                    else
                    {
                        sql =
                            string.Format(
                                "insert into paper_question(paperid,tid,exerciseType,hospitaliD,cent) values({0},{1},{2},{3},{4});",
                                paperid, ids, exerciseType, hospitalId, cent);
                    }
                    var result = dao.ExecuteCommand(sql);
                    UpdatePaperQuestion(paperid, hospitalId);
                    if (result > 0)
                    {
                        message.Status = MessageType.Success;
                        message.Msg = "成功";
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static bool DelPaperQuestion(int id, int paperid, int exerciseType, int hospitalId)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "delete from  paper_question where id =" + id + " and paperid=" + paperid +
                                 " and exerciseType=" + exerciseType + "   and hospitaliD=" + hospitalId + ";";
                    result = dao.ExecuteCommand(sql) > 0;
                    UpdatePaperQuestion(paperid, hospitalId);
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static Message GetPaper(int hospitalId, string name)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select * from papers where HospitalId=" + hospitalId + " and state=1 ";
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += " and name like '%" + name + "%' ";
                    }
                    var result =
                        dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<Paper> papers = new List<Paper>();
                        foreach (DataRow row in result.Rows)
                        {
                            var paper = new Paper { Id = Convert.ToInt32(row["id"]) };
                            if (DBNull.Value != row["Name"])
                            {
                                paper.Name = Convert.ToString(row["Name"]);
                            }
                            if (DBNull.Value != row["Count"])
                            {
                                paper.Count = Convert.ToInt32(row["Count"]);
                            }
                            if (DBNull.Value != row["Duration"])
                            {
                                paper.Duration = Convert.ToInt32(row["Duration"]);
                            }
                            if (DBNull.Value != row["TotalCent"])
                            {
                                paper.TotalCent = Convert.ToInt32(row["TotalCent"]);
                            }
                            if (DBNull.Value != row["DeptCode"])
                            {
                                paper.DeptCode = Convert.ToInt32(row["DeptCode"]);
                            }
                            if (DBNull.Value != row["DeptName"])
                            {
                                paper.DeptName = Convert.ToString(row["DeptName"]);
                            }
                            if (DBNull.Value != row["HospitalId"])
                            {
                                paper.HospitalId = Convert.ToInt32(row["HospitalId"]);
                            }
                            if (DBNull.Value != row["State"])
                            {
                                paper.State = Convert.ToBoolean(row["State"]);
                            }
                            papers.Add(paper);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = papers;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static Message GetPaper(int hospitalId, int id)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var result = dao.GetDataTable("CALL Proc_GetPaper(@PaperId,@HId)",
                        new { PaperId = id, HId = hospitalId });
                    if (result != null && result.Rows.Count > 0)
                    {
                        var paper = new Paper { Id = Convert.ToInt32(result.Rows[0]["id"]) };
                        if (DBNull.Value != result.Rows[0]["Name"])
                        {
                            paper.Name = Convert.ToString(result.Rows[0]["Name"]);
                        }
                        if (DBNull.Value != result.Rows[0]["Count"])
                        {
                            paper.Count = Convert.ToInt32(result.Rows[0]["Count"]);
                        }
                        if (DBNull.Value != result.Rows[0]["Duration"])
                        {
                            paper.Duration = Convert.ToInt32(result.Rows[0]["Duration"]);
                        }
                        if (DBNull.Value != result.Rows[0]["TotalCent"])
                        {
                            paper.TotalCent = Convert.ToInt32(result.Rows[0]["TotalCent"]);
                        }
                        if (DBNull.Value != result.Rows[0]["DeptCode"])
                        {
                            paper.DeptCode = Convert.ToInt32(result.Rows[0]["DeptCode"]);
                        }
                        if (DBNull.Value != result.Rows[0]["DeptName"])
                        {
                            paper.DeptName = Convert.ToString(result.Rows[0]["DeptName"]);
                        }
                        if (DBNull.Value != result.Rows[0]["HospitalId"])
                        {
                            paper.HospitalId = Convert.ToInt32(result.Rows[0]["HospitalId"]);
                        }
                        if (DBNull.Value != result.Rows[0]["State"])
                        {
                            paper.State = Convert.ToBoolean(result.Rows[0]["State"]);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = paper;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static bool DeletePaper(string ids, int userHospitalId)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "update Papers set state=0 where id in (" + ids + ")  and hospitaliD=" + userHospitalId +
                                 ";";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static List<CommonEntity> GetPQuestionStas(int paperId, int hospitalId)
        {
            List<CommonEntity> result = null;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql =
                        "SELECT exerciseType,count(1) as qty,sum(cent) as totalCent FROM paper_question a WHERE a.paperid=" +
                        paperId + " and a.hospitaliD=" + hospitalId + " GROUP BY exerciseType;";
                    var data = dao.GetDataTable(sql);
                    var totalQty = 0;
                    var totalCent = 0;
                    if (data != null && data.Rows.Count > 0)
                    {
                        result = new List<CommonEntity>();
                        var name = "";
                        foreach (DataRow row in data.Rows)
                        {
                            var value = "";
                            if (DBNull.Value != row["exerciseType"])
                            {
                                var et = Convert.ToInt32(row["ExerciseType"]);
                                if (et == 1)
                                {
                                    name = "radioInfo";
                                    value += "单选题";
                                }
                                else if (et == 2)
                                {
                                    name = "multipleChoiceInfo";
                                    value += "多选题";
                                }
                                else if (et == 3)
                                {
                                    name = "recognizedInfo";
                                    value += "判断题";
                                }
                            }
                            if (DBNull.Value != row["qty"])
                            {
                                value += "(共计" + Convert.ToInt32(row["qty"]) + "道题,";
                                totalQty += Convert.ToInt32(row["qty"]);
                            }
                            if (DBNull.Value != row["totalCent"])
                            {
                                value += Convert.ToInt32(row["totalCent"]) + "分)";
                                totalCent += Convert.ToInt32(row["totalCent"]);
                            }
                            var entity = new CommonEntity
                            {
                                Name = name,
                                Value = value
                            };
                            result.Add(entity);
                        }
                        result.Add(new CommonEntity
                        {
                            Name = "paperInfo",
                            Value = string.Format("试卷详情（共计{0}道题，{1}分）", totalQty, totalCent)
                        });
                    }
                }
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        public static bool SavePaper(int paperId, string papername, int totalMini, int hospitalId)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "update Papers set state=1,Name='" + papername + "' ,Duration=" + totalMini +
                              " where id=" + paperId + "  and hospitaliD=" + hospitalId + ";";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static Message GetExam(int hospitalId, string name, string startTime, string endTime)
        {
            List<Test> tests = null;
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select * from exam where state=1 and hosid=" + hospitalId;
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += " and title like '%" + name + "%' ";
                    }
                    if (!string.IsNullOrEmpty(startTime))
                    {
                        sql += " and begintime >= '" + startTime + "' ";
                    }
                    if (!string.IsNullOrEmpty(endTime))
                    {
                        sql += " and endtime <='" + endTime + "' ";
                    }
                    var result = dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        tests = new List<Test>();
                        foreach (DataRow row in result.Rows)
                        {
                            var test = new Test { Id = Convert.ToInt32(row["id"]) };

                            if (DBNull.Value != row["Title"])
                            {
                                test.Title = Convert.ToString(row["Title"]);
                            }
                            if (DBNull.Value != row["Adress"])
                            {
                                test.Adress = Convert.ToString(row["Adress"]);
                            }
                            if (DBNull.Value != row["Begintime"])
                            {
                                test.Begintime = Convert.ToString(row["Begintime"]);
                            }
                            if (DBNull.Value != row["Endtime"])
                            {
                                test.Endtime = Convert.ToString(row["Endtime"]);
                            }
                            if (DBNull.Value != row["Fanwei"])
                            {
                                test.Fanwei = Convert.ToString(row["Fanwei"]);
                            }
                            if (DBNull.Value != row["Fen"])
                            {
                                test.Fen = Convert.ToInt32(row["Fen"]);
                            }
                            if (DBNull.Value != row["groupid"])
                            {
                                test.Groupid = Convert.ToInt32(row["groupid"]);
                            }
                            if (DBNull.Value != row["Groupname"])
                            {
                                test.Groupname = Convert.ToString(row["Groupname"]);
                            }
                            if (DBNull.Value != row["type"])
                            {
                                test.Type = Convert.ToInt32(row["type"]);
                            }
                            if (DBNull.Value != row["hosid"])
                            {
                                test.Hosid = Convert.ToInt32(row["hosid"]);
                            }
                            if (DBNull.Value != row["wardcode"])
                            {
                                test.Wardcode = Convert.ToInt32(row["wardcode"]);
                            }
                            if (DBNull.Value != row["isend"])
                            {
                                test.Isend = Convert.ToInt32(row["isend"]);
                            }
                            if (DBNull.Value != row["jigescore"])
                            {
                                test.Jigescore = Convert.ToInt32(row["jigescore"]);
                            }
                            tests.Add(test);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = tests;
                    }
                }
            }
            catch (Exception ex)
            {
                message.Status = MessageType.Error;
                message.Msg = ex.Message;
            }

            return message;
        }

        public static Message UpdateExamUser(string ids, int examid, int userType, int hospitalId)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "未更新任何数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "";
                    if (userType == 0) //参加考试的人
                    {
                        if (ids.Contains(","))
                        {
                            var arr = ids.Split(',');
                            foreach (var item in arr)
                            {
                                sql +=
                                    string.Format(
                                        "insert into testuser(testid,userid,ischeck,fen,status) values({0},{1},{2},{3},{4});",
                                        examid, item, 0, 0, 0);
                            }
                        }
                        else
                        {
                            sql =
                                string.Format(
                                    "insert into testuser(testid,userid,ischeck,fen,status) values({0},{1},{2},{3},{4});",
                                    examid, ids, 0, 0, 0);
                        }
                    }
                    else
                    {
                        if (ids.Contains(","))
                        {
                            var arr = ids.Split(',');
                            foreach (var item in arr)
                            {
                                sql +=
                                    string.Format(
                                        "insert into jiankao(testid,userid) values({0},{1});", examid, item);
                            }
                        }
                        else
                        {
                            sql =
                                string.Format(
                                    "insert into jiankao(testid,userid) values({0},{1});", examid, ids);
                        }
                    }
                    var result = dao.ExecuteCommand(sql);
                    if (result > 0)
                    {
                        message.Status = MessageType.Success;
                        message.Msg = "成功";
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static Message GetEndUserForExam(int examid)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select * from user where id in(select userid from jiankao where testid=" + examid + ")";
                    var result = dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<EndUser> endUsers = new List<EndUser>();
                        foreach (DataRow row in result.Rows)
                        {
                            var endUser = new EndUser
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Name = Convert.ToString(row["name"]),
                                Phone = Convert.ToString(row["Phone"]),
                                LoginId = Convert.ToString(row["LoginId"]),
                                Hospitalcode = Convert.ToInt32(row["Hospitalcode"]),
                                Hospitalname = Convert.ToString(row["Hospitalname"]),
                                Deptcode = Convert.ToInt32(row["Deptcode"]),
                                Deptname = Convert.ToString(row["Deptname"]),
                                Zccode = Convert.ToInt32(row["Zccode"]),
                                Zcname = Convert.ToString(row["Zcname"]),
                                Gwcode = Convert.ToInt32(row["Gwcode"]),
                                Gwname = Convert.ToString(row["Gwname"]),
                                Lvcode = Convert.ToInt32(row["Lvcode"]),
                                Lvname = Convert.ToString(row["Lvname"]),
                            };
                            if (DBNull.Value != row["Decode"])
                            {
                                endUser.Decode = Convert.ToInt32(row["Decode"]);
                            }
                            if (DBNull.Value != row["Dename"])
                            {
                                endUser.Dename = Convert.ToString(row["Dename"]);
                            }
                            if (DBNull.Value != row["teamid"])
                            {
                                endUser.Xzcode = Convert.ToString(row["teamid"]);
                            }
                            if (DBNull.Value != row["teamname"])
                            {
                                endUser.Xzname = Convert.ToString(row["teamname"]);
                            }
                            endUsers.Add(endUser);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = endUsers;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static Message GetKsUserForExam(int examid, int hospitalid, string name, string loginId, string deptname,
            string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname,
            string xzcode, string xzname)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select * from user where id in(select userid from testuser where testid=" + examid + ")";
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += "  and name like '%" + name + "%'  ";
                    }
                    if (!string.IsNullOrEmpty(loginId))
                    {
                        sql += "  and  loginId like '%" + loginId + "%'  ";
                    }
                    if (!string.IsNullOrEmpty(deptcode) && !deptcode.Equals("-1"))
                    {
                        sql += "   and deptcode ='" + deptcode + "'  ";
                    }
                    if (!string.IsNullOrEmpty(gwcode) && !gwcode.Equals("-1"))
                    {
                        sql += "   and gwcode ='" + gwcode + "'  ";
                    }
                    if (!string.IsNullOrEmpty(zccode) && !zccode.Equals("-1"))
                    {
                        sql += "   and zccode ='" + zccode + "'  ";
                    }
                    if (!string.IsNullOrEmpty(lvcode) && !lvcode.Equals("-1"))
                    {
                        sql += "   and lvcode ='" + lvcode + "'  ";
                    }
                    if (!string.IsNullOrEmpty(xzname) && !xzcode.Equals("-1"))
                    {
                        sql += "   and teamid like '%" + xzcode + ",%'  ";
                    }

                    var result =
                        dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<EndUser> endUsers = new List<EndUser>();
                        foreach (DataRow row in result.Rows)
                        {
                            var endUser = new EndUser
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Name = Convert.ToString(row["name"]),
                                Phone = Convert.ToString(row["Phone"]),
                                LoginId = Convert.ToString(row["LoginId"]),
                                Hospitalcode = Convert.ToInt32(row["Hospitalcode"]),
                                Hospitalname = Convert.ToString(row["Hospitalname"]),
                                Deptcode = Convert.ToInt32(row["Deptcode"]),
                                Deptname = Convert.ToString(row["Deptname"]),
                                Zccode = Convert.ToInt32(row["Zccode"]),
                                Zcname = Convert.ToString(row["Zcname"]),
                                Gwcode = Convert.ToInt32(row["Gwcode"]),
                                Gwname = Convert.ToString(row["Gwname"]),
                                Lvcode = Convert.ToInt32(row["Lvcode"]),
                                Lvname = Convert.ToString(row["Lvname"]),
                            };
                            if (DBNull.Value != row["Decode"])
                            {
                                endUser.Decode = Convert.ToInt32(row["Decode"]);
                            }
                            if (DBNull.Value != row["Dename"])
                            {
                                endUser.Dename = Convert.ToString(row["Dename"]);
                            }
                            if (DBNull.Value != row["teamid"])
                            {
                                endUser.Xzcode = Convert.ToString(row["teamid"]);
                            }
                            if (DBNull.Value != row["teamname"])
                            {
                                endUser.Xzname = Convert.ToString(row["teamname"]);
                            }
                            endUsers.Add(endUser);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = endUsers;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static Boolean DeleteExamUser(int id, int userType, int examid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql;
                    if (userType == 0)
                    {
                        sql = "delete from testuser where testid=" + examid + " and userid=" + id;
                    }
                    else
                    {
                        sql = "delete from jiankao where testid=" + examid + " and userid=" + id;
                    }
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static Message GetExam(int hospitalId, int id)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var result = dao.GetDataTable("CALL Proc_GetExam(@ExamId,@HId)", new { ExamId = id, HId = hospitalId });
                    if (result != null && result.Rows.Count > 0)
                    {
                        var row = result.Rows[0];
                        var test = new Test { Id = Convert.ToInt32(row["id"]) };

                        if (DBNull.Value != row["Title"])
                        {
                            test.Title = Convert.ToString(row["Title"]);
                        }
                        if (DBNull.Value != row["Adress"])
                        {
                            test.Adress = Convert.ToString(row["Adress"]);
                        }
                        if (DBNull.Value != row["Begintime"])
                        {
                            test.Begintime = Convert.ToString(row["Begintime"]);
                        }
                        if (DBNull.Value != row["Endtime"])
                        {
                            test.Endtime = Convert.ToString(row["Endtime"]);
                        }
                        if (DBNull.Value != row["Fanwei"])
                        {
                            test.Fanwei = Convert.ToString(row["Fanwei"]);
                        }
                        if (DBNull.Value != row["Fen"])
                        {
                            test.Fen = Convert.ToInt32(row["Fen"]);
                        }
                        if (DBNull.Value != row["groupid"])
                        {
                            test.Groupid = Convert.ToInt32(row["groupid"]);
                        }
                        if (DBNull.Value != row["Groupname"])
                        {
                            test.Groupname = Convert.ToString(row["Groupname"]);
                        }
                        if (DBNull.Value != row["type"])
                        {
                            test.Type = Convert.ToInt32(row["type"]);
                        }
                        if (DBNull.Value != row["hosid"])
                        {
                            test.Hosid = Convert.ToInt32(row["hosid"]);
                        }
                        if (DBNull.Value != row["wardcode"])
                        {
                            test.Wardcode = Convert.ToInt32(row["wardcode"]);
                        }
                        if (DBNull.Value != row["isend"])
                        {
                            test.Isend = Convert.ToInt32(row["isend"]);
                        }
                        if (DBNull.Value != row["jigescore"])
                        {
                            test.Jigescore = Convert.ToInt32(row["jigescore"]);
                        }
                        if (DBNull.Value != row["PaperId"])
                        {
                            test.PaperId = Convert.ToInt32(row["PaperId"]);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = test;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static List<CommonEntityViewModel> GetPaper(int hospitalId)
        {
            List<CommonEntityViewModel> papers = null;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select * from papers where HospitalId=" + hospitalId + " and state=1 ";
                    var result =
                        dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        papers = new List<CommonEntityViewModel>();
                        foreach (DataRow row in result.Rows)
                        {
                            var paper = new CommonEntityViewModel();
                            if (DBNull.Value != row["Name"])
                            {
                                paper.Name = Convert.ToString(row["Name"]);
                            }
                            if (DBNull.Value != row["Id"])
                            {
                                paper.Value = Convert.ToInt32(row["id"]);
                            }
                            if (DBNull.Value != row["TotalCent"])
                            {
                                paper.Type = Convert.ToString(row["TotalCent"]);
                            }
                            papers.Add(paper);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return papers;
        }

        public static bool UpdateExam(int id, string title, string address, string beginTime, string endTime,
            string paper, string jigescore, int hospitalId)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    int paperid = 0;
                    int fen = 0;
                    if (paper.Contains("-"))
                    {
                        var arr = paper.Split('-');
                        paperid = Convert.ToInt32(arr[0]);
                        fen = Convert.ToInt32(arr[1]);
                    }
                    var sql = "update exam set state=1,title='" + title + "',adress='" + address + "',endtime='" +
                              endTime + "',begintime='" + beginTime + "',fen=" + fen + ",jigescore=" + jigescore +
                              ",paperid=" +
                              paperid + " where id=" + id + "  and hosid=" + hospitalId + ";";
                    sql += "DELETE FROM paper WHERE paperid=" + id + ";";
                    sql += "INSERT INTO paper(paperid, questionid, seq, isright, useranswer, score)SELECT " + id +
                           " as paperid,tid as questionid,0 as seq,0 as isright,'' as useranswer,cent FROM paper_question where paperid=" +
                           paperid + "; ";
                    result = dao.ExecuteCommand(sql) > 0 && AddNotice(id, title, hospitalId);
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static bool AddNotice(int testid, string testname, int hid)
        {
            bool result = true;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select count(1) from notice where testcode=" + testid + "; ";
                    var c = dao.GetInt(sql);
                    if (c <= 0)
                    {
                        sql = "INSERT INTO `group` (`name`,hospitalcode) VALUES('" + testname + "'," + hid +
                              ");select @@IDENTITY;";
                        var gid = dao.GetInt(sql);
                        sql = "INSERT INTO `usergroup`(userid,groupid) SELECT userid," + gid +
                              " as groupid FROM testuser WHERE testid=" + testid + ";";
                        sql +=
                            "INSERT INTO `notice`(content,type,educode,testcode,sendtime,hospitalcode,groupid,isvalued) VALUES('您有一个考试通知待确认', 1, 0, " +
                            testid + ", '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + hid + ", " + gid +
                            ", 0); ";
                        return dao.ExecuteCommand(sql) > 0;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }

            return result;
        }

        public static Message GetTrain(string name, string orgname, int orgtype, int hospitalId)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "select * from edu where state=1 and hosid=" + hospitalId;
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += " and zhuti like '%" + name + "%' ";
                    }
                    if (!string.IsNullOrEmpty(orgname))
                    {
                        sql += " and org like '%" + orgname + "%' ";
                    }
                    if (orgtype > -1)
                    {
                        sql += " and type=" + orgtype;
                    }
                    var result = dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        var list = new List<Train>();
                        foreach (DataRow row in result.Rows)
                        {
                            var train = new Train { Id = Convert.ToInt32(row["id"]) };
                            if (DBNull.Value != row["zhuti"])
                            {
                                train.Zhuti = Convert.ToString(row["zhuti"]);
                            }
                            if (DBNull.Value != row["Org"])
                            {
                                train.Org = Convert.ToString(row["Org"]);
                            }
                            if (DBNull.Value != row["Adress"])
                            {
                                train.Adress = Convert.ToString(row["Adress"]);
                            }
                            if (DBNull.Value != row["Time"])
                            {
                                train.Time = Convert.ToString(row["Time"]);
                            }
                            if (DBNull.Value != row["Recordtime"])
                            {
                                train.Recordtime = Convert.ToString(row["Recordtime"]);
                            }
                            if (DBNull.Value != row["Type"])
                            {
                                train.Type = Convert.ToInt32(row["Type"]);
                            }
                            if (DBNull.Value != row["Hosid"])
                            {
                                train.Hosid = Convert.ToInt32(row["Hosid"]);
                            }
                            if (DBNull.Value != row["Teacher"])
                            {
                                train.Teacher = Convert.ToString(row["Teacher"]);
                            }
                            list.Add(train);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = list;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }
            return message;
        }

        public static Message GetTrain(int hospitalId, int trainId)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var result = dao.GetDataTable("CALL Proc_GetTrain(@EDUID,@HID)", new { EDUID = trainId, HID = hospitalId });
                    if (result != null && result.Rows.Count > 0)
                    {
                        var row = result.Rows[0];
                        var train = new Train { Id = Convert.ToInt32(row["id"]) };
                        if (DBNull.Value != row["zhuti"])
                        {
                            train.Zhuti = Convert.ToString(row["zhuti"]);
                        }
                        if (DBNull.Value != row["Org"])
                        {
                            train.Org = Convert.ToString(row["Org"]);
                        }
                        if (DBNull.Value != row["Adress"])
                        {
                            train.Adress = Convert.ToString(row["Adress"]);
                        }
                        if (DBNull.Value != row["Time"])
                        {
                            train.Time = Convert.ToString(row["Time"]);
                        }
                        if (DBNull.Value != row["Recordtime"])
                        {
                            train.Recordtime = Convert.ToString(row["Recordtime"]);
                        }
                        if (DBNull.Value != row["Type"])
                        {
                            train.Type = Convert.ToInt32(row["Type"]);
                        }
                        if (DBNull.Value != row["Hosid"])
                        {
                            train.Hosid = Convert.ToInt32(row["Hosid"]);
                        }
                        if (DBNull.Value != row["Teacher"])
                        {
                            train.Teacher = Convert.ToString(row["Teacher"]);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = train;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }
            return message;
        }

        public static Message GetKsUser(int examid, int hospitalid, string name, string loginId, string deptname,
            string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname,
            string xzcode, string xzname)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select * from user where id in(SELECT USERID FROM `eduuser` WHERE EDUID=" + examid + ")";
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += "  and name like '%" + name + "%'  ";
                    }
                    if (!string.IsNullOrEmpty(loginId))
                    {
                        sql += "  and  loginId like '%" + loginId + "%'  ";
                    }
                    if (!string.IsNullOrEmpty(deptcode) && !deptcode.Equals("-1"))
                    {
                        sql += "   and deptcode ='" + deptcode + "'  ";
                    }
                    if (!string.IsNullOrEmpty(gwcode) && !gwcode.Equals("-1"))
                    {
                        sql += "   and gwcode ='" + gwcode + "'  ";
                    }
                    if (!string.IsNullOrEmpty(zccode) && !zccode.Equals("-1"))
                    {
                        sql += "   and zccode ='" + zccode + "'  ";
                    }
                    if (!string.IsNullOrEmpty(lvcode) && !lvcode.Equals("-1"))
                    {
                        sql += "   and lvcode ='" + lvcode + "'  ";
                    }
                    if (!string.IsNullOrEmpty(xzname) && !xzcode.Equals("-1"))
                    {
                        sql += "   and teamid like '%" + xzcode + ",%'  ";
                    }

                    var result =
                        dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<EndUser> endUsers = new List<EndUser>();
                        foreach (DataRow row in result.Rows)
                        {
                            var endUser = new EndUser
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Name = Convert.ToString(row["name"]),
                                Phone = Convert.ToString(row["Phone"]),
                                LoginId = Convert.ToString(row["LoginId"]),
                                Hospitalcode = Convert.ToInt32(row["Hospitalcode"]),
                                Hospitalname = Convert.ToString(row["Hospitalname"]),
                                Deptcode = Convert.ToInt32(row["Deptcode"]),
                                Deptname = Convert.ToString(row["Deptname"]),
                                Zccode = Convert.ToInt32(row["Zccode"]),
                                Zcname = Convert.ToString(row["Zcname"]),
                                Gwcode = Convert.ToInt32(row["Gwcode"]),
                                Gwname = Convert.ToString(row["Gwname"]),
                                Lvcode = Convert.ToInt32(row["Lvcode"]),
                                Lvname = Convert.ToString(row["Lvname"]),
                            };
                            if (DBNull.Value != row["Decode"])
                            {
                                endUser.Decode = Convert.ToInt32(row["Decode"]);
                            }
                            if (DBNull.Value != row["Dename"])
                            {
                                endUser.Dename = Convert.ToString(row["Dename"]);
                            }
                            if (DBNull.Value != row["teamid"])
                            {
                                endUser.Xzcode = Convert.ToString(row["teamid"]);
                            }
                            if (DBNull.Value != row["teamname"])
                            {
                                endUser.Xzname = Convert.ToString(row["teamname"]);
                            }
                            endUsers.Add(endUser);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = endUsers;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static Message UpdateTrainUser(string ids, int examid, int hospitalId)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "未更新任何数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "";
                        if (ids.Contains(","))
                        {
                            var arr = ids.Split(',');
                            foreach (var item in arr)
                            {
                                sql +=
                                    string.Format(
                                        "insert into eduuser(eduid,userid,ischeck) values({0},{1},{2});",
                                        examid, item, 0);
                            }
                        }
                        else
                        {
                            sql =
                                string.Format(
                                    "insert into eduuser(eduid,userid,ischeck) values({0},{1},{2});",
                                    examid, ids, 0);
                        }
                   
                    var result = dao.ExecuteCommand(sql);
                    if (result > 0)
                    {
                        message.Status = MessageType.Success;
                        message.Msg = "成功";
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static Boolean DeleteTrainUser(int id, int examid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql= "delete from eduuser where eduid=" + examid + " and userid=" + id;
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static Message GetTrainDetail(int examid)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "select * from edukc where eduid="+examid;
                    var result = dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        var list = new List<EduKc>();
                        foreach (DataRow row in result.Rows)
                        {
                            var train = new EduKc { Id = Convert.ToInt32(row["id"]) };
                            if (DBNull.Value != row["Title"])
                            {
                                train.Title = Convert.ToString(row["Title"]);
                            }
                            if (DBNull.Value != row["Tec"])
                            {
                                train.Tec = Convert.ToString(row["Tec"]);
                            }
                            if (DBNull.Value != row["EduId"])
                            {
                                train.EduId = Convert.ToInt32(row["EduId"]);
                            }
                            list.Add(train);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = list;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }
            return message;
        }

        public static Boolean DelTrainDetail(int id, int examid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "delete from edukc where eduid=" + examid + " and id=" + id;
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static bool UpdateTrain(int trainid,string zhuti, string address, string org, string time, string teacher, int score,int type,int hospitalId)
        {
            string sql = "update edu set zhuti='"+zhuti+"',org='"+org+"',adress='"+address+"',time='"+time+"',recordtime=NOW(),score="+score+",type="+ type + ",teacher='"+teacher+"',state=1 where id=" + trainid+" and hosid="+hospitalId+";";
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    return (dao.ExecuteCommand(sql)>0)&&(AddTrainNotice(trainid,zhuti,hospitalId));
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool UpdateTrainDetail(int trainid,string title,string tec)
        {
            string sql = "insert into edukc(title,tec,eduid) values('"+title+"','"+tec+"',"+trainid+");";
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    return dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Boolean DelTrainFj(int id,int type, int examid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql;
                    if (type == 0)
                    {
                        sql = "delete from edukj where eduid=" + examid + " and id=" + id;
                    }
                    else
                    {
                        sql = "delete from edusp where eduid=" + examid + " and id=" + id;
                    }
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static Message GetTrainFj(int examid)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "当前没有数据",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql =
                        "SELECT id,spname as name,1 as type,eduid FROM `edusp` WHERE eduid=" + examid + " UNION SELECT id,kjname as name,0 as type,eduid FROM `edukj` WHERE eduid=" + examid+";";
                    var result = dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        var list = new List<FuJian>();
                        foreach (DataRow row in result.Rows)
                        {
                            var train = new FuJian { Id = Convert.ToInt32(row["id"]) };
                            if (DBNull.Value != row["Name"])
                            {
                                train.Name = Convert.ToString(row["Name"]);
                            }
                            if (DBNull.Value != row["Type"])
                            {
                                train.Type = Convert.ToInt32(row["Type"]);
                            }
                            if (DBNull.Value != row["EduId"])
                            {
                                train.EduId = Convert.ToInt32(row["EduId"]);
                            }
                            list.Add(train);
                        }
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = list;
                    }
                }
            }
            catch (Exception)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }
            return message;
        }

        public static bool AddTrainFj(int eduid, string name, string url,int type)
        {
            string sql = "";
            if (type == 0)
            {
                sql = "insert into edukj(kjname,kjurl,eduid) values('"+name+"','"+url+"',"+eduid+");";
            }
            else
            {
                sql = "insert into edusp(spname,spurl,eduid) values('" + name + "','" + url + "'," + eduid + ");";
            }
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    return dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool AddTrainNotice(int eduid, string name, int hid)
        {
            bool result = true;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "select count(1) from notice where educode=" + eduid + "; ";
                    var c = dao.GetInt(sql);
                    if (c <= 0)
                    {
                        sql = "INSERT INTO `group` (`name`,hospitalcode) VALUES('" + name + "'," + hid +
                              ");select @@IDENTITY;";
                        var gid = dao.GetInt(sql);
                        sql = "INSERT INTO `usergroup`(userid,groupid) SELECT userid," + gid +
                              " as groupid FROM eduuser WHERE eduid=" + eduid + ";";
                        sql +=
                            "INSERT INTO `notice`(content,type,educode,testcode,sendtime,hospitalcode,groupid,isvalued) VALUES('您有一个考试通知待确认',2, "+eduid+",0, '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " + hid + ", " + gid +
                            ", 0); ";
                        return dao.ExecuteCommand(sql) > 0;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }

            return result;
        }

        public static bool DeleteTrain(int trainid, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql= "update edu set state=0 where id=" + trainid + " and hosid=" + hospitalid;
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static bool DelExam(string ids, int userHospitalId)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "update exam set state=0 where id in (" + ids + ")  and hosid=" + userHospitalId +
                                 ";";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static bool DelTrain(string ids, int userHospitalId)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "update edu set state=0 where id in (" + ids + ")  and hosid=" + userHospitalId +
                                 ";";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }
    }
}