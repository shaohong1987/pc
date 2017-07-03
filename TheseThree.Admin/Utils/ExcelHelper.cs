using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
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
                                            var dept = list.FirstOrDefault(it => it.Type == "ks" && it.Name == deptname);
                                            var deptcode = -1;
                                            if (dept != null)
                                            {
                                                deptcode = dept.Value;
                                            }
                                            if (zcList.Contains(zc))
                                            {
                                                var zhichen = list.FirstOrDefault(it => it.Type == "zc" && it.Name == zc);
                                                var zccode = -1;
                                                if (zhichen != null)
                                                {
                                                    zccode = zhichen.Value;
                                                }
                                                if (gwList.Contains(gw))
                                                {
                                                    var gangwei =
                                                        list.FirstOrDefault(it => it.Type == "gw" && it.Name == zc);
                                                    var gwcode = -1;
                                                    if (gangwei != null)
                                                    {
                                                        gwcode = gangwei.Value;
                                                    }
                                                    if (lvList.Contains(gw))
                                                    {
                                                        var level =
                                                            list.FirstOrDefault(it => it.Type == "lv" && it.Name == zc);
                                                        var lvcode = -1;
                                                        if (level != null)
                                                        {
                                                            gwcode = level.Value;
                                                        }

                                                        if (deList.Contains(gw))
                                                        {
                                                            var degree =
                                                                list.FirstOrDefault(
                                                                    it => it.Type == "lv" && it.Name == zc);
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
                                                                            xzcode=xiaozhu.Value+"";
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
                                        subject = subject.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        acase = acase.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        options = options.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        desc = desc.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        label = label.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        var ops = GetOptions(options);
                                        GetLabel(label, ref labels, ref newLabels);
                                        var exerciseType = 1;
                                        //在此执行sql，写入数据
                                        if (answer.Length > 2)
                                        {
                                            exerciseType = ops.Count > 2 ? 2 : 3;
                                        }
                                        sql += string.Format("insert into timu(answer,exerciseType,itemA,itemB,itemC,itemD,itemE,itemF,itemG,itemH,itemI,itemJ,itemNum,anli,question,remark,section,label,difficulty) values('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},'{13}','{14}','{15}','{16}','{17}',{18});",
                                            answer, exerciseType, (ops.Count > 0 ? ops[ops.Count - 1] : ""), (ops.Count > 1 ? ops[ops.Count - 2] : ""), (ops.Count > 2 ? ops[ops.Count - 3] : ""), (ops.Count > 3 ? ops[ops.Count - 4] : ""), (ops.Count > 4 ? ops[ops.Count - 5] : ""), (ops.Count > 5 ? ops[ops.Count - 6] : ""), (ops.Count > 6 ? ops[ops.Count - 7] : ""), (ops.Count > 7 ? ops[ops.Count - 8] : ""), (ops.Count > 8 ? ops[ops.Count - 9] : ""), (ops.Count > 9 ? ops[ops.Count - 10] : ""), ops.Count, acase, subject, desc, sectionId, label, (string.IsNullOrEmpty(difficulty) ? "0" : difficulty));
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

                                    sql = "update section set count=(select count(1) from timu where section=" + sectionId + ") where id=" + sectionId + " and hospitalid=" + hospitalId + ";";
                                    foreach (var label in newLabels)
                                    {
                                        sql += string.Format("insert into label(content,sectionId,hospitalId) values('{0}',{1},{2});", label, sectionId, hospitalId);
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
                        var sheet = workbook.GetSheetAt(1);
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
                                        !string.IsNullOrEmpty(answer) && (!string.IsNullOrEmpty(itemA)|| !string.IsNullOrEmpty(itemB)|| !string.IsNullOrEmpty(itemC) || !string.IsNullOrEmpty(itemD) || !string.IsNullOrEmpty(itemE) || !string.IsNullOrEmpty(itemF) || !string.IsNullOrEmpty(itemG) || !string.IsNullOrEmpty(itemH) || !string.IsNullOrEmpty(itemI) || !string.IsNullOrEmpty(itemJ) ))
                                    {
                                        subject = subject.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        acase = acase.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        desc = desc.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        label = label.Replace("\"", "\\\"").Replace("\'", "\\\'");
                                        List<string> ops=new List<string>();
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
                                                    (ops.Count > 9 ? ops[ops.Count - 10] : ""), ops.Count, acase,
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
                                if (!string.IsNullOrEmpty(sql))
                                {
                                    var message = TeachingModel.AddTimu(sql);
                                    if (message.Status != MessageType.Success)
                                    {
                                        errList.Add(message.Msg);
                                    }

                                    sql = "update section set count=(select count(1) from timu where section=" + sectionId + ") where id=" + sectionId + " and hospitalid=" + hospitalId + ";";
                                    foreach (var label in newLabels)
                                    {
                                        sql += string.Format("insert into label(content,sectionId,hospitalId) values('{0}',{1},{2});", label, sectionId, hospitalId);
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

        public static List<string> GetOptions(string option)
        {
            List<string> result = new List<string>();
            var e = Regex.Matches(option, "E、(.*)");
            string temp = "";
            foreach (Match item in e)
            {
                temp = item.Groups[0].Value;
                result.Add(item.Groups[0].Value);
            }
            if (!string.IsNullOrEmpty(temp))
                option = option.Replace(temp, "");

            var d = Regex.Matches(option, "D、(.*)");
            foreach (Match item in d)
            {
                temp = item.Groups[0].Value;
                result.Add(item.Groups[0].Value);
            }
            if (!string.IsNullOrEmpty(temp))
                option = option.Replace(temp, "");

            var c = Regex.Matches(option, "C、(.*)");
            foreach (Match item in c)
            {
                temp = item.Groups[0].Value;
                result.Add(item.Groups[0].Value);
            }
            if (!string.IsNullOrEmpty(temp))
                option = option.Replace(temp, "");

            var b = Regex.Matches(option, "B、(.*)");
            foreach (Match item in b)
            {
                temp = item.Groups[0].Value;
                result.Add(item.Groups[0].Value);
            }
            if (!string.IsNullOrEmpty(temp))
                option = option.Replace(temp, "");

            var a = Regex.Matches(option, "A、(.*)");
            foreach (Match item in a)
            {
                result.Add(item.Groups[0].Value);
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
                    if (!labels.Contains(item))
                    {
                        newLabels.Add(item);
                        labels.Add(item);
                    }
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
    }
}