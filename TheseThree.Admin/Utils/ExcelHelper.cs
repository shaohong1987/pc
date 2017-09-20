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

        
        public static bool ExportDataToExcel(List<ExamInfoDetail> data,ExamAllInfo data0,User user, List<EndUser> data1, string sheetName, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            IWorkbook workbook = null;
           
            if(fileName.EndsWith(".xlsx"))
                workbook = new XSSFWorkbook();
            if (fileName.EndsWith(".xls"))
                workbook = new HSSFWorkbook();
            try
            {
                ISheet sheet;
                ICellStyle style;
                ICellStyle style0;//水平居中
                ICellStyle style1;//水平居中,背景色，加粗
             
                IFont font;
                IFont font0;
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                    style = workbook.CreateCellStyle();
                    style0 = workbook.CreateCellStyle();
                    style1 = workbook.CreateCellStyle();
                    font = workbook.CreateFont();
                    font0 = workbook.CreateFont();//加粗
                }
                else
                {
                    return false;
                }
                var row0 = sheet.CreateRow(0);
                row0.Height = 50 * 20;
                
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                font.Boldweight = short.MaxValue;
                font.FontHeightInPoints = 20;
                font.FontName = "宋体";
                style.SetFont(font);
                row0.CreateCell(0).SetCellValue(user.HospitalName);//表头
                row0.Cells[0].CellStyle = style;
                var row1 = sheet.CreateRow(1);
                row1.Height = 30 * 20;
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 9));
                style0.Alignment = HorizontalAlignment.Center;
                style0.VerticalAlignment = VerticalAlignment.Center;
                style0.BorderBottom = BorderStyle.Thin;
                style0.BorderLeft = BorderStyle.Thin;
                style0.BorderRight = BorderStyle.Thin;
                style0.BorderTop = BorderStyle.Thin;
                string str = "考试名称:" + data0.ExamName + "             考试级别:" + data0.ExamStyle + "              考试时间:" + data0.ExamTime + "";
                row1.CreateCell(0).SetCellValue(str);//描述
                row0.Cells[0].CellStyle = style;
                row1.Cells[0].CellStyle = style0;
                ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(new CellRangeAddress(0,0, 0, 9), BorderStyle.Medium, HSSFColor.Black.Index);
                ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(new CellRangeAddress(1, 1, 0, 9), BorderStyle.Medium, HSSFColor.Black.Index);
                
                var row = sheet.CreateRow(2);
                style1.Alignment = HorizontalAlignment.Center;
                style1.VerticalAlignment = VerticalAlignment.Center;
                style1.BorderBottom = BorderStyle.Thin;
                style1.BorderLeft = BorderStyle.Thin;
                style1.BorderRight = BorderStyle.Thin;
                style1.BorderTop = BorderStyle.Thin;
                style1.FillForegroundColor = HSSFColor.SkyBlue.Index;
                style1.FillPattern = FillPattern.SolidForeground;
                font0.Boldweight = short.MaxValue;
                style1.SetFont(font0);
                row.CreateCell(0).SetCellValue("类型");
                row.CreateCell(1).SetCellValue("月份");
                row.CreateCell(2).SetCellValue("内容");
                row.CreateCell(3).SetCellValue("科室");
                row.CreateCell(4).SetCellValue("工号");
                row.CreateCell(5).SetCellValue("姓名");
                row.CreateCell(6).SetCellValue("成绩");
                row.CreateCell(7).SetCellValue("备注");
                row.CreateCell(8).SetCellValue("出勤");
                row.CreateCell(9).SetCellValue("缺席原因");
                row.Cells[0].CellStyle = style1;
                row.Cells[1].CellStyle = style1;
                row.Cells[2].CellStyle = style1;
                row.Cells[3].CellStyle = style1;
                row.Cells[4].CellStyle = style1;
                row.Cells[5].CellStyle = style1;
                row.Cells[6].CellStyle = style1;
                row.Cells[7].CellStyle = style1;
                row.Cells[8].CellStyle = style1;
                row.Cells[9].CellStyle = style1;
                int r = 0;
                int good = 0;
                for (var i = 0; i < data.Count; i++)
                {
                    row = sheet.CreateRow(i + 3);
                    row.CreateCell(0).SetCellValue(data[i].Level);
                    row.CreateCell(1).SetCellValue(data[i].Month);
                    row.CreateCell(2).SetCellValue(data[i].Content);
                    row.CreateCell(3).SetCellValue(data[i].Dept);
                    row.CreateCell(4).SetCellValue(data[i].LoginId);
                    row.CreateCell(5).SetCellValue(data[i].Name);
                    row.CreateCell(6).SetCellValue(data[i].Score);
                    row.CreateCell(7).SetCellValue(data[i].Remark);
                    row.CreateCell(8).SetCellValue(data[i].Attend);
                    row.CreateCell(9).SetCellValue(data[i].AttendRemark);
                    row.Cells[0].CellStyle = style0;
                    row.Cells[1].CellStyle = style0;
                    row.Cells[2].CellStyle = style0;
                    row.Cells[3].CellStyle = style0;
                    row.Cells[4].CellStyle = style0;
                    row.Cells[5].CellStyle = style0;
                    row.Cells[6].CellStyle = style0;
                    row.Cells[7].CellStyle = style0;
                    row.Cells[8].CellStyle = style0;
                    row.Cells[9].CellStyle = style0;
                    if (data[i].Remark.Equals("及格"))
                        good++;
                    r = i+3;
                }
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 9));
                sheet.AddMergedRegion(new CellRangeAddress(r+1, r+1, 0, 9));
                string str0 = "";
                if (data1 != null && data1.Count > 0)
                {
                    foreach(EndUser u in data1)
                    {
                        str0 += u.Name + "    ";
                    }
                }
                row = sheet.CreateRow(r+1);
                row.CreateCell(0).SetCellValue("监考人员:" + str0);
                row = sheet.CreateRow(r + 2);
                sheet.AddMergedRegion(new CellRangeAddress(r + 2, r + 2, 0, 9));
                string str1 = "应参加人数:"+data0.shouldCome +"    实际参加人数:"+ data0.realCome + "    缺席人数:"+ data0.unCome +"\n     合格人数:"+ good +"    合格分:"+ data0.jigeScore +"    合格率:"+ good*100/data.Count+"%";
                row.CreateCell(0).SetCellValue("统计:"+str1);
                row = sheet.CreateRow(r + 3);
                sheet.AddMergedRegion(new CellRangeAddress(r + 3, r + 3, 0,7));
                sheet.AddMergedRegion(new CellRangeAddress(r + 3, r + 3,8, 9));
                row.CreateCell(0).SetCellValue("制表人:"+ user.UserName);
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                row.CreateCell(8).SetCellValue("制表日期:" + time);
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