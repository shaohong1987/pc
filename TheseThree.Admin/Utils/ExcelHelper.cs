using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using TheseThree.Admin.Models;
using TheseThree.Admin.Models.Entities;

namespace TheseThree.Admin.Utils
{
    public class ExcelHelper
    {
        //批量导入用户
        public static List<string> ImportEndUsers(string filePath, int hospitalId, string hospitalname)
        {
            List<RegSms> regSmses = new List<RegSms>();
            List<string> phones = new List<string>();
            List<string> errList = new List<string>();
            FileStream fs = null;
            IWorkbook workbook = null;
            var list = new OrganizationModel().GetCommonAttr(hospitalId);
            if (list == null)
            {
                errList.Add("请先维护科室，职称，职称、层级、学历，小组信息。");
                return errList;
            }
            var ksList = list.Where(it => it.Type == "ks").Select(it => it.Name).ToList();
            var gwList = list.Where(it => it.Type == "gw").Select(it => it.Name).ToList();
            var zcList = list.Where(it => it.Type == "zc").Select(it => it.Name).ToList();
            var lvList = list.Where(it => it.Type == "lv").Select(it => it.Name).ToList();
            var deList = list.Where(it => it.Type == "de").Select(it => it.Name).ToList();
            var xzList = list.Where(it => it.Type == "xz").Select(it => it.Name).ToList();
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    if (filePath.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                        workbook = new XSSFWorkbook(fs);
                    else if (filePath.IndexOf(".xls", StringComparison.Ordinal) > 0)
                        workbook = new HSSFWorkbook(fs);
                    if (workbook != null)
                    {
                        var sheet = workbook.GetSheetAt(0);
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;
                            if (rowCount > 0)
                            {
                                string sql = "";
                                for (var i = 1; i <= rowCount; i++)
                                {
                                    string phone = GetCellValue(sheet.GetRow(i).GetCell(0));
                                    string name = GetCellValue(sheet.GetRow(i).GetCell(1));
                                    string loginId = GetCellValue(sheet.GetRow(i).GetCell(2));
                                    string deptname = GetCellValue(sheet.GetRow(i).GetCell(3));
                                    string zc = GetCellValue(sheet.GetRow(i).GetCell(4));
                                    string gw = GetCellValue(sheet.GetRow(i).GetCell(5));
                                    string lv = GetCellValue(sheet.GetRow(i).GetCell(6));
                                    string de = GetCellValue(sheet.GetRow(i).GetCell(7));
                                    string xz = GetCellValue(sheet.GetRow(i).GetCell(8));
                                    if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(name) &&
                                        !string.IsNullOrEmpty(loginId) && !string.IsNullOrEmpty(deptname) &&
                                        !string.IsNullOrEmpty(zc) && !string.IsNullOrEmpty(gw) &&
                                        !string.IsNullOrEmpty(lv) && !string.IsNullOrEmpty(de))
                                    {
                                        if (ksList.Contains(deptname))
                                        {
                                            var dept = list.FirstOrDefault(
                                                it => it.Type == "ks" && it.Name == deptname);
                                            var deptcode = -1;
                                            if (dept != null)
                                            {
                                                deptcode = dept.Value;
                                            }
                                            if (zcList.Contains(zc))
                                            {
                                                var zhichen =
                                                    list.FirstOrDefault(it => it.Type == "zc" && it.Name == zc);
                                                var zccode = -1;
                                                if (zhichen != null)
                                                {
                                                    zccode = zhichen.Value;
                                                }
                                                if (gwList.Contains(gw))
                                                {
                                                    var gangwei =
                                                        list.FirstOrDefault(it => it.Type == "gw" && it.Name == gw);
                                                    var gwcode = -1;
                                                    if (gangwei != null)
                                                    {
                                                        gwcode = gangwei.Value;
                                                    }
                                                    if (lvList.Contains(lv))
                                                    {
                                                        var level =
                                                            list.FirstOrDefault(it => it.Type == "lv" && it.Name == lv);
                                                        var lvcode = -1;
                                                        if (level != null)
                                                        {
                                                            lvcode = level.Value;
                                                        }

                                                        if (deList.Contains(de))
                                                        {
                                                            var degree =
                                                                list.FirstOrDefault(
                                                                    it => it.Type == "de" && it.Name == de);
                                                            var decode = -1;
                                                            if (degree != null)
                                                            {
                                                                decode = degree.Value;
                                                            }

                                                            var flag = true;
                                                            string xzcode = "";
                                                            if (!string.IsNullOrEmpty(xz))
                                                            {
                                                                xz = xz.Replace("，", ",");
                                                                if (xz.Contains(","))
                                                                {
                                                                    var arr = xz.Split(',');
                                                                    foreach (var s in arr)
                                                                    {
                                                                        if (!xzList.Contains(s))
                                                                        {
                                                                            flag = false;
                                                                            break;
                                                                        }
                                                                        var xiaozhu =
                                                                            list.FirstOrDefault(
                                                                                it => it.Type == "xz" && it.Name == s);
                                                                        if (xiaozhu != null)
                                                                            xzcode += "," + xiaozhu.Value;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (!xzList.Contains(xz))
                                                                    {
                                                                        flag = false;
                                                                    }
                                                                    else
                                                                    {
                                                                        var xiaozhu =
                                                                            list.FirstOrDefault(
                                                                                it => it.Type == "xz" && it.Name == xz);
                                                                        if (xiaozhu != null)
                                                                            xzcode = xiaozhu.Value + "";
                                                                    }
                                                                }
                                                            }

                                                            if (flag)
                                                            {
                                                                if (xzcode.StartsWith(","))
                                                                {
                                                                    xzcode = xzcode.Substring(1);
                                                                }
                                                                if (!UserModel.CheckEndUserExist(phone))
                                                                {
                                                                    if (!phones.Contains(phone))
                                                                    {
                                                                        phones.Add(phone);
                                                                        sql +=
                                                                            string.Format(
                                                                                "insert into user(loginID,password,name,phone,token,hospitalcode,hospitalname,wardcode,wardname,deptcode,deptname,card,gwcode,gwname,zccode,zcname,lvcode,lvname,decode,dename,isvalued,iostoken,type,teamid,teamname) values('{0}','{2}','{1}','{2}','',{3},'{4}',-1,'',{5},'{6}','',{7},'{8}',{9},'{10}',{11},'{12}',{13},'{14}',0,'',1,'{15}','{16}');",
                                                                                loginId, name, phone, hospitalId,
                                                                                hospitalname,
                                                                                deptcode,
                                                                                deptname, gwcode, gw, zccode, zc,
                                                                                lvcode, lv, decode, de, xzcode, xz);
                                                                        regSmses.Add(new RegSms()
                                                                        {
                                                                            Name = name,
                                                                            HosName = hospitalname,
                                                                            Phone = phone
                                                                        });
                                                                    }
                                                                    else
                                                                    {
                                                                        errList.Add("第" + i + "行数据用户已存在，未写入数据库。");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    errList.Add("第" + i + "行数据用户已存在，未写入数据库。");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                errList.Add("第" + i + "行数据 \"小组\"不存在，未写入数据库。");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            errList.Add("第" + i + "行数据 \"学历\"不存在，未写入数据库。");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        errList.Add("第" + i + "行数据 \"层级\"不存在，未写入数据库。");
                                                    }
                                                }
                                                else
                                                {
                                                    errList.Add("第" + i + "行数据 \"职务\"不存在，未写入数据库。");
                                                }
                                            }
                                            else
                                            {
                                                errList.Add("第" + i + "行数据 \"职称\"不存在，未写入数据库。");
                                            }
                                        }
                                        else
                                        {
                                            errList.Add("第" + i + "行数据 \"科室\"不存在，未写入数据库。");
                                        }
                                    }
                                    else
                                    {
                                        errList.Add("第" + i + "行数据不完整，未写入数据库。");
                                    }
                                }
                                if (!string.IsNullOrEmpty(sql))
                                {
                                    var message = UserModel.AddEndUser(sql);
                                    HttpHelper.SendSms(regSmses);
                                    if (message.Status != MessageType.Success)
                                    {

                                        errList.Add(message.Msg);
                                    }
                                }
                                else
                                {
                                    errList.Add("没有适合导入的数据!");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errList.Add(ex.Message);
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return errList;
        }

        /// <summary>
        /// exerciseType:1单选，2多选，3判断，4简答
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="hospitalId"></param>
        /// <param name="sectionId"></param>
        /// <param name="labels"></param>
        /// <returns></returns>
        public static List<string> ImportSubject2(string filePath, int hospitalId, int sectionId, List<string> labels)
        {
            List<string> newLabels = new List<string>();
            List<string> errList = new List<string>();
            FileStream fs = null;
            IWorkbook workbook = null;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    if (filePath.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                        workbook = new XSSFWorkbook(fs);
                    else if (filePath.IndexOf(".xls", StringComparison.Ordinal) > 0)
                        workbook = new HSSFWorkbook(fs);
                    if (workbook != null)
                    {
                        var sheet = workbook.GetSheetAt(0);
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;
                            if (rowCount > 0)
                            {
                                string sql = "";
                                for (var i = 1; i <= rowCount; i++)
                                {
                                    string subject = GetCellValue(sheet.GetRow(i).GetCell(0));
                                    string acase = GetCellValue(sheet.GetRow(i).GetCell(1));
                                    string options = GetCellValue(sheet.GetRow(i).GetCell(2));
                                    string answer = GetCellValue(sheet.GetRow(i).GetCell(3));
                                    string difficulty = GetCellValue(sheet.GetRow(i).GetCell(4));
                                    string desc = GetCellValue(sheet.GetRow(i).GetCell(5));
                                    string label = GetCellValue(sheet.GetRow(i).GetCell(7));
                                    if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(options) &&
                                        !string.IsNullOrEmpty(answer))
                                    {
                                        subject = subject.Trim().Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        acase = acase.Trim().Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        options = options.Trim().Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        desc = desc.Trim().Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        label = label.Trim().Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        var ops = GetOptions(options);
                                        GetLabel(label, ref labels, ref newLabels);
                                        var exerciseType = 1;
                                        //在此执行sql，写入数据
                                        if (answer.Length > 2)
                                        {
                                            exerciseType = ops.Count > 2 ? 2 : 3;
                                        }
                                        sql += string.Format(
                                            "insert into timu(answer,exerciseType,itemA,itemB,itemC,itemD,itemE,itemF,itemG,itemH,itemI,itemJ,itemNum,anli,question,remark,section,label,difficulty) values('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},'{13}','{14}','{15}','{16}','{17}',{18});",
                                            answer, exerciseType, (ops.Count > 0 ? ops[ops.Count - 1] : ""),
                                            (ops.Count > 1 ? ops[ops.Count - 2] : ""),
                                            (ops.Count > 2 ? ops[ops.Count - 3] : ""),
                                            (ops.Count > 3 ? ops[ops.Count - 4] : ""),
                                            (ops.Count > 4 ? ops[ops.Count - 5] : ""),
                                            (ops.Count > 5 ? ops[ops.Count - 6] : ""),
                                            (ops.Count > 6 ? ops[ops.Count - 7] : ""),
                                            (ops.Count > 7 ? ops[ops.Count - 8] : ""),
                                            (ops.Count > 8 ? ops[ops.Count - 9] : ""),
                                            (ops.Count > 9 ? ops[ops.Count - 10] : ""), ops.Count, acase, subject, desc,
                                            sectionId, label, (string.IsNullOrEmpty(difficulty) ? "0" : difficulty));
                                    }
                                    else
                                    {
                                        errList.Add("第" + i + "行数据不完整，未写入数据库。");
                                    }
                                }
                                if (!string.IsNullOrEmpty(sql))
                                {
                                    var message = TeachingModel.AddTimu(sql);
                                    if (message.Status != MessageType.Success)
                                    {
                                        errList.Add(message.Msg);
                                    }

                                    sql = "update section set count=(select count(1) from timu where section=" +
                                          sectionId + ") where id=" + sectionId + " and hospitalid=" + hospitalId + ";";
                                    foreach (var label in newLabels)
                                    {
                                        sql += string.Format(
                                            "insert into label(content,sectionId,hospitalId) values('{0}',{1},{2});",
                                            label, sectionId, hospitalId);
                                    }
                                    message = TeachingModel.AddTimu(sql);
                                    if (message.Status != MessageType.Success)
                                    {
                                        errList.Add(message.Msg);
                                    }
                                }
                                else
                                {
                                    errList.Add("没有适合导入的数据!");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errList.Add(ex.Message);
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return errList;
        }

        public static List<string> ImportSubject(string filePath, int hospitalId, int sectionId, List<string> labels)
        {
            List<string> newLabels = new List<string>();
            List<string> errList = new List<string>();
            FileStream fs = null;
            IWorkbook workbook = null;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    if (filePath.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                        workbook = new XSSFWorkbook(fs);
                    else if (filePath.IndexOf(".xls", StringComparison.Ordinal) > 0)
                        workbook = new HSSFWorkbook(fs);
                    if (workbook != null)
                    {
                        var sheet = workbook.GetSheetAt(0);
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;
                            if (rowCount > 0)
                            {
                                string sql = "";
                                for (var i = 1; i <= rowCount; i++)
                                {
                                    string subject = GetCellValue(sheet.GetRow(i).GetCell(1));
                                    string acase = GetCellValue(sheet.GetRow(i).GetCell(0));
                                    string type = GetCellValue(sheet.GetRow(i).GetCell(13));
                                    string answer = GetCellValue(sheet.GetRow(i).GetCell(12));
                                    string difficulty = GetCellValue(sheet.GetRow(i).GetCell(14));
                                    string desc = GetCellValue(sheet.GetRow(i).GetCell(15));
                                    string label = GetCellValue(sheet.GetRow(i).GetCell(16));
                                    string itemA = GetCellValue(sheet.GetRow(i).GetCell(2));
                                    string itemB = GetCellValue(sheet.GetRow(i).GetCell(3));
                                    string itemC = GetCellValue(sheet.GetRow(i).GetCell(4));
                                    string itemD = GetCellValue(sheet.GetRow(i).GetCell(5));
                                    string itemE = GetCellValue(sheet.GetRow(i).GetCell(6));
                                    string itemF = GetCellValue(sheet.GetRow(i).GetCell(7));
                                    string itemG = GetCellValue(sheet.GetRow(i).GetCell(8));
                                    string itemH = GetCellValue(sheet.GetRow(i).GetCell(9));
                                    string itemI = GetCellValue(sheet.GetRow(i).GetCell(10));
                                    string itemJ = GetCellValue(sheet.GetRow(i).GetCell(11));
                                    if (!string.IsNullOrEmpty(subject) &&
                                        !string.IsNullOrEmpty(answer) &&
                                        (!string.IsNullOrEmpty(itemA) || !string.IsNullOrEmpty(itemB) ||
                                         !string.IsNullOrEmpty(itemC) || !string.IsNullOrEmpty(itemD) ||
                                         !string.IsNullOrEmpty(itemE) || !string.IsNullOrEmpty(itemF) ||
                                         !string.IsNullOrEmpty(itemG) || !string.IsNullOrEmpty(itemH) ||
                                         !string.IsNullOrEmpty(itemI) || !string.IsNullOrEmpty(itemJ)))
                                    {
                                        subject = subject.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        acase = acase.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        desc = desc.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        label = label.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        List<string> ops = new List<string>();
                                        if (!string.IsNullOrEmpty(itemA))
                                        {
                                            ops.Add(itemA.Replace("\"", "\\\"").Replace("\'", "\\\'"));
                                        }
                                        if (!string.IsNullOrEmpty(itemB))
                                        {
                                            ops.Add(itemB.Replace("\"", "\\\"").Replace("\'", "\\\'"));
                                        }
                                        if (!string.IsNullOrEmpty(itemC))
                                        {
                                            ops.Add(itemC.Replace("\"", "\\\"").Replace("\'", "\\\'"));
                                        }
                                        if (!string.IsNullOrEmpty(itemD))
                                        {
                                            ops.Add(itemD.Replace("\"", "\\\"").Replace("\'", "\\\'"));
                                        }
                                        if (!string.IsNullOrEmpty(itemE))
                                        {
                                            ops.Add(itemE.Replace("\"", "\\\"").Replace("\'", "\\\'"));
                                        }
                                        if (!string.IsNullOrEmpty(itemF))
                                        {
                                            ops.Add(itemF.Replace("\"", "\\\"").Replace("\'", "\\\'"));
                                        }
                                        if (!string.IsNullOrEmpty(itemG))
                                        {
                                            ops.Add(itemG.Replace("\"", "\\\"").Replace("\'", "\\\'"));
                                        }
                                        if (!string.IsNullOrEmpty(itemH))
                                        {
                                            ops.Add(itemH.Replace("\"", "\\\"").Replace("\'", "\\\'"));
                                        }
                                        if (!string.IsNullOrEmpty(itemI))
                                        {
                                            ops.Add(itemI.Replace("\"", "\\\"").Replace("\'", "\\\'"));
                                        }
                                        if (!string.IsNullOrEmpty(itemJ))
                                        {
                                            ops.Add(itemJ.Replace("\"", "\\\"").Replace("\'", "\\\'"));
                                        }
                                        GetLabel(label, ref labels, ref newLabels);
                                        var exerciseType = -1;
                                        switch (type)
                                        {
                                            case "单选题":
                                                exerciseType = 1;
                                                break;
                                            case "多选题":
                                                exerciseType = 2;
                                                break;
                                            case "判断题":
                                                exerciseType = 3;
                                                break;
                                        }
                                        if (exerciseType > 0)
                                        {
                                            sql +=
                                                string.Format(
                                                    "insert into timu(answer,exerciseType,itemA,itemB,itemC,itemD,itemE,itemF,itemG,itemH," +
                                                    "itemI,itemJ,itemNum,anli,question,remark,section,label,difficulty) " +
                                                    "values('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},'{13}'" +
                                                    ",'{14}','{15}','{16}','{17}',{18});",
                                                    answer, exerciseType, (ops.Count > 0 ? ops[0] : ""),
                                                    (ops.Count > 1 ? ops[1] : ""),
                                                    (ops.Count > 2 ? ops[2] : ""),
                                                    (ops.Count > 3 ? ops[3] : ""),
                                                    (ops.Count > 4 ? ops[4] : ""),
                                                    (ops.Count > 5 ? ops[5] : ""),
                                                    (ops.Count > 6 ? ops[6] : ""),
                                                    (ops.Count > 7 ? ops[7] : ""),
                                                    (ops.Count > 8 ? ops[8] : ""),
                                                    (ops.Count > 9 ? ops[9] : ""), ops.Count, acase,
                                                    subject, desc, sectionId, label,
                                                    (string.IsNullOrEmpty(difficulty) ? "0" : difficulty));
                                        }
                                        else
                                        {
                                            errList.Add("第" + i + "行数据题型不存在，未写入数据库。");
                                        }
                                    }
                                    else
                                    {
                                        errList.Add("第" + i + "行数据不完整，未写入数据库。");
                                    }
                                }
                                if (!(errList.Count > 0 && errList != null))
                                {
                                    if (!string.IsNullOrEmpty(sql))
                                    {
                                        var message = TeachingModel.AddTimu(sql);
                                        if (message.Status != MessageType.Success)
                                        {
                                            errList.Add(message.Msg);
                                        }

                                        sql = "update section set count=(select count(1) from timu where section=" +
                                              sectionId + ") where id=" + sectionId + " and hospitalid=" + hospitalId +
                                              ";";
                                        foreach (var label in newLabels)
                                        {
                                            sql += string.Format(
                                                "insert into label(content,sectionId,hospitalId) values('{0}',{1},{2});",
                                                label, sectionId, hospitalId);
                                        }
                                        message = TeachingModel.AddTimu(sql);
                                        if (message.Status != MessageType.Success)
                                        {
                                            errList.Add(message.Msg);
                                        }
                                    }
                                    else
                                    {
                                        errList.Add("没有适合导入的数据!");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errList.Add(ex.Message);
                if (fs != null)
                {
                    fs.Close();
                }
            }
            return errList;
        }

        public static List<string> GetOptions(string option)
        {
            List<string> result = new List<string>();
            var e = Regex.Matches(option, "E、(.*)");
            string temp = "";
            foreach (Match item in e)
            {
                temp = item.Groups[0].Value;
                result.Add(item.Groups[0].Value.Replace("E、", ""));
            }
            if (!string.IsNullOrEmpty(temp))
                option = option.Replace(temp, "");

            var d = Regex.Matches(option, "D、(.*)");
            foreach (Match item in d)
            {
                temp = item.Groups[0].Value;
                result.Add(item.Groups[0].Value.Replace("D、", ""));
            }
            if (!string.IsNullOrEmpty(temp))
                option = option.Replace(temp, "");

            var c = Regex.Matches(option, "C、(.*)");
            foreach (Match item in c)
            {
                temp = item.Groups[0].Value;
                result.Add(item.Groups[0].Value.Replace("C、", ""));
            }
            if (!string.IsNullOrEmpty(temp))
                option = option.Replace(temp, "");

            var b = Regex.Matches(option, "B、(.*)");
            foreach (Match item in b)
            {
                temp = item.Groups[0].Value;
                result.Add(item.Groups[0].Value.Replace("B、", ""));
            }
            if (!string.IsNullOrEmpty(temp))
                option = option.Replace(temp, "");

            var a = Regex.Matches(option, "A、(.*)");
            foreach (Match item in a)
            {
                result.Add(item.Groups[0].Value.Replace("A、", ""));
            }
            return result;
        }

        public static void GetLabel(string label, ref List<string> labels, ref List<string> newLabels)
        {
            if (label.Contains("，"))
            {
                label = label.Replace("，", ",");
                var arr = label.Split(',');
                foreach (var item in arr)
                {
                    if (!labels.Contains(item) && !"三基本".Equals(item))
                    {
                        newLabels.Add(item);
                        labels.Add(item);
                    }
                }
            }
            else
            {
                if (!labels.Contains(label))
                {
                    newLabels.Add(label);
                    labels.Add(label);
                }
            }
        }

        public static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return "";
            string result = "";
            switch (cell.CellType)
            {
                case CellType.Blank:
                    break;
                case CellType.Numeric:
                    short format = cell.CellStyle.DataFormat;
                    if (format == 14 || format == 31 || format == 57 || format == 58)
                        result = cell.DateCellValue.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        result = cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                    break;
                case CellType.String:
                    result = cell.StringCellValue;
                    break;
            }
            return result.Trim();
        }

        public static int DataTableToExcel(System.Data.DataTable data, string sheetName, bool isColumnWritten, string fileName)
        {
            IWorkbook workbook = null;
            var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls", StringComparison.Ordinal) > 0)
                workbook = new HSSFWorkbook();
            try
            {
                ISheet sheet;
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }
                int count;
                int j;
                if (isColumnWritten)
                {
                    var row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }
                int i;
                for (i = 0; i < data.Rows.Count; ++i)
                {
                    var row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs);
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        public static bool ExportDataToExcel(List<ExamInfoDetail> data, string sheetName, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            IWorkbook workbook = null;

            if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls", StringComparison.Ordinal) > 0)
                workbook = new HSSFWorkbook();
            try
            {
                ISheet sheet;
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return false;
                }
                IRow row1 = sheet.CreateRow(0);
                ICell cell = row1.CreateCell(0);
                cell.SetCellValue("医院名称");
                ICellStyle style = workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                IFont font = workbook.CreateFont();
                font.Boldweight = short.MaxValue;
                font.FontHeight = 400;
                style.SetFont(font);
                cell.CellStyle = style;
                row1.Height = 30 * 20;
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));

                ICellStyle style2 = workbook.CreateCellStyle();
                style2.VerticalAlignment = VerticalAlignment.Center;
                IRow row2 = sheet.CreateRow(1);
                row2.Height = 30 * 20;
                ICell cell0 = row2.CreateCell(0);
                cell0.SetCellValue("考试名称:");
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 2));
                ICell cell1 = row2.CreateCell(3);
                cell1.SetCellValue("考试级别:");
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 3, 5));
                ICell cell2 = row2.CreateCell(6);
                cell2.SetCellValue("考试时间:");
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 6, 8));
                cell0.CellStyle = cell1.CellStyle = cell2.CellStyle = style2;

                ICellStyle style3 = workbook.CreateCellStyle();
                style3.Alignment = HorizontalAlignment.Center;
                style3.FillForegroundColor = HSSFColor.SeaGreen.Index;
                style3.FillPattern = FillPattern.SolidForeground;
                style3.BorderBottom = style3.BorderTop = style3.BorderLeft = style3.BorderRight = BorderStyle.Medium;
                var row = sheet.CreateRow(2);
                ICell icell0 = row.CreateCell(0);
                icell0.SetCellValue("序号");
                icell0.CellStyle = style3;

                ICell icell1 = row.CreateCell(1);
                icell1.SetCellValue("科室");
                icell1.CellStyle = style3;

                ICell icell2 = row.CreateCell(2);
                icell2.SetCellValue("工号");
                icell2.CellStyle = style3;

                ICell icell3 = row.CreateCell(3);
                icell3.SetCellValue("姓名");
                icell3.CellStyle = style3;

                ICell icell4 = row.CreateCell(4);
                icell4.SetCellValue("层级");
                icell4.CellStyle = style3;

                ICell icell5 = row.CreateCell(5);
                icell5.SetCellValue("成绩");
                icell5.CellStyle = style3;

                ICell icell6 = row.CreateCell(6);
                icell6.SetCellValue("合格");
                icell6.CellStyle = style3;

                ICell icell7 = row.CreateCell(7);
                icell7.SetCellValue("缺席原因");
                icell7.CellStyle = style3;

                ICell icell8 = row.CreateCell(8);
                icell8.SetCellValue("备注");
                icell8.CellStyle = style3;

                ICellStyle style4 = workbook.CreateCellStyle();
                style4.BorderBottom = style4.BorderTop = style4.BorderLeft = style4.BorderRight = BorderStyle.Medium;
                for (var i = 0; i < data.Count; i++)
                {
                    row = sheet.CreateRow(i + 3);
                    ICell iiCell0 = row.CreateCell(0); iiCell0.SetCellValue((i + 1) + "");
                    ICell iiCell1 = row.CreateCell(1); iiCell1.SetCellValue(data[i].Dept);
                    ICell iiCell2 = row.CreateCell(2); iiCell2.SetCellValue(data[i].LoginId);
                    ICell iiCell3 = row.CreateCell(3); iiCell3.SetCellValue(data[i].Name);
                    ICell iiCell4 = row.CreateCell(4); iiCell4.SetCellValue(data[i].Name);
                    ICell iiCell5 = row.CreateCell(5); iiCell5.SetCellValue(data[i].Score);
                    ICell iiCell6 = row.CreateCell(6); iiCell6.SetCellValue(data[i].Remark);
                    ICell iiCell7 = row.CreateCell(7); iiCell7.SetCellValue(data[i].AttendRemark);
                    ICell iiCell8 = row.CreateCell(8); iiCell8.SetCellValue("");
                    iiCell8.CellStyle = iiCell7.CellStyle = iiCell6.CellStyle = iiCell5.CellStyle = iiCell4.CellStyle = iiCell3.CellStyle = iiCell2.CellStyle = iiCell1.CellStyle = iiCell0.CellStyle = style4;
                }

                IRow row3 = sheet.CreateRow(data.Count + 2);
                ICell cell3 = row3.CreateCell(0);
                cell3.SetCellValue("监考人员:");
                CellRangeAddress region1 = new CellRangeAddress(data.Count + 2, data.Count + 2, 0, 8);
                sheet.AddMergedRegion(region1);
                ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region1, BorderStyle.Medium, HSSFColor.Black.Index);

                ICellStyle style6 = workbook.CreateCellStyle();
                style6.VerticalAlignment = VerticalAlignment.Center;
                style6.WrapText = true;
                IRow row4 = sheet.CreateRow(data.Count + 3);
                ICell cell4 = row4.CreateCell(0);
                row4.Height = 30 * 20;
                cell4.SetCellValue("统计:   应参加人数:            实际参加人数:            缺席人数:            \r\n          合格人数：              合格分:                      合格率:            ");
                cell4.CellStyle = style6;
                CellRangeAddress region2 = new CellRangeAddress(data.Count + 3, data.Count + 3, 0, 8);
                sheet.AddMergedRegion(region2);
                ((HSSFSheet) sheet).SetEnclosedBorderOfRegion(region2, BorderStyle.Medium, HSSFColor.Black.Index);

                IRow row5 = sheet.CreateRow(data.Count + 4);
                ICell cell5 = row5.CreateCell(0);
                cell5.SetCellValue("制表人:");
                sheet.AddMergedRegion(new CellRangeAddress(data.Count + 4, data.Count + 4, 0, 3));

                ICellStyle style7 = workbook.CreateCellStyle();
                style7.Alignment = HorizontalAlignment.Right;
                ICell cell6 = row5.CreateCell(4);
                cell6.SetCellValue("制表日期:"+DateTime.Now.ToString("yyyy-MM-dd"));
                cell6.CellStyle = style7;
                sheet.AddMergedRegion(new CellRangeAddress(data.Count + 4, data.Count + 4, 4, 8));
                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    workbook.Write(fs);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static void ExportDataToWord(List<ExamInfoDetail> data, object fileName)
        {
            if (File.Exists(fileName.ToString()))
            {
                File.Delete(fileName.ToString());
            }
            object none = Missing.Value;
            var wordApp = new ApplicationClass();
            var document = wordApp.Documents.Add(ref none, ref none, ref none, ref none);

            wordApp.Selection.ParagraphFormat.LineSpacing = 15f;
            document.Paragraphs.Last.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            document.Paragraphs.Last.Range.Font.Size = 15;
            document.Paragraphs.Last.Range.Font.Bold = 1;
            document.Paragraphs.Last.Range.Text = data[0].Content + "\n";
            document.Paragraphs.Last.Range.Font.Bold = 0;
            document.Paragraphs.Last.Range.Font.Size = 11;

            //document.Paragraphs.Last.Range.InsertBreak();
            document.Paragraphs.Last.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

            object unite = WdUnits.wdStory;
            document.Content.InsertAfter("\n"); //这一句与下一句的顺序不能颠倒，原因还没搞透
            wordApp.Selection.EndKey(ref unite, ref none); //将光标移动到文档末尾
            wordApp.Selection.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

            var table = document.Tables.Add(wordApp.Selection.Range, data.Count + 1, 9, ref none, ref none);
            table.Borders.Enable = 1;
            table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
            try
            {
                table.Cell(1, 1).Range.Text = "类型";
                table.Cell(1, 2).Range.Text = "月份";
                table.Cell(1, 3).Range.Text = "科室";
                table.Cell(1, 4).Range.Text = "工号";
                table.Cell(1, 5).Range.Text = "姓名";
                table.Cell(1, 6).Range.Text = "成绩";
                table.Cell(1, 7).Range.Text = "备注";
                table.Cell(1, 8).Range.Text = "出勤";
                table.Cell(1, 9).Range.Text = "缺席原因";
                for (var i = 0; i < data.Count; i++)
                {
                    table.Cell(i + 2, 1).Range.Text = data[i].Level;
                    table.Cell(i + 2, 2).Range.Text = data[i].Month;
                    table.Cell(i + 2, 3).Range.Text = data[i].Dept;
                    table.Cell(i + 2, 4).Range.Text = data[i].LoginId;
                    table.Cell(i + 2, 5).Range.Text = data[i].Name;
                    table.Cell(i + 2, 6).Range.Text = data[i].Score.ToString();
                    table.Cell(i + 2, 7).Range.Text = data[i].Remark;
                    table.Cell(i + 2, 8).Range.Text = data[i].Attend;
                    table.Cell(i + 2, 9).Range.Text = data[i].AttendRemark;
                }

                var pns = wordApp.Selection.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterEvenPages]
                    .PageNumbers;
                pns.NumberStyle = WdPageNumberStyle.wdPageNumberStyleNumberInDash;
                pns.HeadingLevelForChapter = 0;
                pns.IncludeChapterNumber = false;
                pns.RestartNumberingAtSection = false;
                pns.StartingNumber = 0;
                object pagenmbetal = WdPageNumberAlignment.wdAlignPageNumberCenter;
                object first = true;
                wordApp.Selection.Sections[1].Footers[WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers
                    .Add(ref pagenmbetal, ref first);

                document.SaveAs(ref fileName, ref none, ref none, ref none, ref none, ref none, ref none, ref none,
                    ref none, ref none, ref none, ref none, ref none, ref none, ref none, ref none);
                document.Close(ref none, ref none, ref none);
                wordApp.Quit(ref none, ref none, ref none);
            }
            catch (Exception e)
            {
                //ignore
                document.Close(ref none, ref none, ref none);
                wordApp.Quit(ref none, ref none, ref none);
            }
        }
    }
}