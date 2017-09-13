using System;
using System.Collections.Generic;
using System.Data;
using TheseThree.Admin.DataAccess;
using TheseThree.Admin.Models.Entities;

namespace TheseThree.Admin.Models
{
    public class NoticeModel
    {
        public static Message GetNotice(string name,int hospitalcode)
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
                    var sql = "SELECT a.*,b.`name` as groupname FROM notice a LEFT JOIN `group` b ON a.groupid=b.id where a.isvalued=0 and a.hospitalcode=" + hospitalcode;
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += "  and a.content like '%" + name + "%'  ";
                    }
                    var result =
                        dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<Notice> notices = new List<Notice>();
                        foreach (DataRow row in result.Rows)
                        {
                            var notice = new Notice
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Content = Convert.ToString(row["content"]),
                                Type = Convert.ToInt32(row["type"]),
                                Educode = Convert.ToInt32(row["educode"]),
                                Testcode = Convert.ToInt32(row["testcode"]),
                                Sendtime = Convert.ToString(row["sendtime"]),
                                Hospitalcode = Convert.ToInt32(row["Hospitalcode"]),
                                Groupid = Convert.ToInt32(row["groupid"]),
                                Isvalued = Convert.ToInt32(row["isvalued"]),
                                GroupName = Convert.ToString(row["groupname"])
                            };
                            notices.Add(notice);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = notices;
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

        public static bool DeleteNotice(string id, int hospitalcode)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "update notice set isvalued=1 where id="+id+" and hospitalcode="+hospitalcode+";";
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