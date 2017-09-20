using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TheseThree.Admin.DataAccess;
using TheseThree.Admin.Models.Entities;
using TheseThree.Admin.Models.ViewModels;
using TheseThree.Admin.Utils;

namespace TheseThree.Admin.Models
{
    public class UserModel
    {
        public static Message ValidateUser(LoginViewModel model)
        {
            var message = new Message
            {
                Status = MessageType.Fail,
                Msg = "用户名或密码错误",
                Data = null
            };
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var result = dao.GetDataTable("CALL Proc_Admin_Login(@id,@pwd)", new { id = model.UserName, pwd = model.Password });
                    if (result != null && result.Rows.Count > 0)
                    {
                        var row = result.Rows[0];
                        var user = new User
                        {
                            UserName = Convert.ToString(row["username"]),
                            HospitalEndDate = Convert.ToString(row["enddate"]),
                            HospitalId = Convert.ToInt32(row["hospitalId"]),
                            HospitalName = Convert.ToString(row["hospitalname"]),
                            HospitalRegDate = Convert.ToString(row["regdate"]),
                            UserId = Convert.ToInt32(row["userid"]),
                            UserType = Convert.ToInt32(row["usertype"]),
                            DeptCode = Convert.ToInt32(row["deptcode"]),
                            DeptName = DBNull.Value != row["DeptName"] ? Convert.ToString(row["DeptName"]) : ""
                        };
                        message.Status = MessageType.Success;
                        message.Msg = "登陆成功";
                        message.Data = user;
                    }
                }
            }
            catch (Exception ex)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }
        public static Message GetEndUsers(int hospitalid, string name, string phone, string loginId, string deptname, string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname, string decode, string dename, string xzcode, string xzname, int deptCode, int userType)
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
                    var sql = "select * from user where Isvalued=0 and Hospitalcode=" + hospitalid;
                    if (!string.IsNullOrEmpty(name))
                    {
                        sql += "  and name like '%" + name + "%'  ";
                    }
                    if (!string.IsNullOrEmpty(phone))
                    {
                        sql += "   and phone like '%" + phone + "%'  ";
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
                    if (!string.IsNullOrEmpty(decode) && !decode.Equals("-1"))
                    {
                        sql += "   and decode ='" + decode + "'  ";
                    }
                    if (!string.IsNullOrEmpty(xzcode) && !xzcode.Equals("-1"))
                    {
                        sql += "   and teamid like '%" + xzcode + "%'  ";
                    }
                    if (userType == 3)//科室管理员
                    {
                        sql += " and deptcode =" + deptCode;
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
                                Decode = Convert.ToInt32(row["Decode"]),
                                Dename = Convert.ToString(row["Dename"]),
                                Xzcode = Convert.ToString(row["teamid"]),
                                Xzname = Convert.ToString(row["teamname"])
                            };
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
        public static Message GetEndUsers(int examid, int hospitalid, string name, string loginId, string deptname, string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname, string xzcode, string xzname, int deptCode, int userType)
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
                    var sql = "select * from user where Isvalued=0 and Hospitalcode=" + hospitalid;
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
                    if (userType == 3)//科室管理员
                    {
                        sql += " and deptcode =" + deptCode;
                    }
                    sql +=
                        "  and id NOT IN (SELECT userid FROM testuser a WHERE a.testid=" + examid + " UNION SELECT userid FROM jiankao a WHERE a.testid=" + examid + ")";
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
        public static Message GetEndUsersForRole(int roleid, int hospitalid, string name, string loginId, string deptname, string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname, string xzcode, string xzname)
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
                    var sql = "select * from user where Isvalued=0 and Hospitalcode=" + hospitalid;
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
                    sql +=
                        "  and loginID NOT IN (SELECT username FROM admin_user a WHERE a.usertype=" + roleid + " and a.hospitalid=" + hospitalid + ")";
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
        public static Message GetEndUsersForTrain(int examid, int hospitalid, string name, string loginId, string deptname, string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname, string xzcode, string xzname)
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
                    var sql = "select * from user where Isvalued=0 and Hospitalcode=" + hospitalid;
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
                    sql +=
                        "  and id NOT IN (SELECT USERID FROM `eduuser` WHERE EDUID=" + examid + ")";
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
        public static bool CheckEndUserExist(string phone)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    result = dao.GetInt("select count(*) from user where phone=@n and isvalued=0", new { n = phone }) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }
        public static Message UpdateEndUser(int id, string name, string phone, string loginId, string deptname, string deptcode, string gwcode, string gwname, string zccode, string zcname, string lvcode, string lvname, string decode, string dename, string xzcode, string xzname, int hospitalid, string hospitalname)
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
                        sql = string.Format("update user set loginid='{0}',name='{1}',deptcode={2},deptname='{3}',gwcode={4},gwname='{5}',zccode={6},zcname='{7}',lvcode={8},lvname='{9}',decode={10},dename='{11}',teamid='{14}',teamname='{15}' where hospitalcode={12} and id={13}", loginId, name, deptcode, deptname, gwcode, gwname, zccode, zcname, lvcode, lvname, decode, dename, hospitalid, id, xzcode, xzname);

                    }
                    else
                    {
                        sql =
                            string.Format(
                                "insert into user(loginID,password,name,phone,token,hospitalcode,hospitalname,wardcode,wardname,deptcode,deptname,card,gwcode,gwname,zccode,zcname,lvcode,lvname,decode,dename,isvalued,iostoken,type,teamid,teamname) values('{0}','{2}','{1}','{2}','',{3},'{4}',-1,'',{5},'{6}','',{7},'{8}',{9},'{10}',{11},'{12}',{13},'{14}',0,'',1,'{15}','{16}');", loginId, name, phone, hospitalid, hospitalname, deptcode, deptname, gwcode, gwname, zccode, zcname, lvcode, lvname, decode, dename, xzcode, xzname);
                    }
                    var result = dao.ExecuteCommand(sql);
                    if (id <= 0)
                    {
                        var regSmses = new List<RegSms>
                        {
                            new RegSms
                            {
                                Name = name,
                                HosName = hospitalname,
                                Phone = phone
                            }
                        };
                        HttpHelper.SendSms(regSmses);
                    }
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
        public static Message AddEndUser(string sql)
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
        public static Message ResetPwd(string id, int hospitalid)
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
                    string sql = string.Format("update user set password=phone where hospitalcode={0} and id in ({1})", hospitalid, id);
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
        public static Message DelUser(string id, int hospitalid)
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
                    string sql = string.Format("update user set isvalued=1 where hospitalcode={0} and id in ({1})", hospitalid, id);
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
        public static bool UpdatePwd(int userid, string op, string np)
        {
            using (var dao = TheseThreeDao.GetInstance())
            {
                string sql = string.Format("update admin_user set userpwd='{0}' where id={1} and userpwd='{2}';", np, userid, op);
                int result = dao.ExecuteCommand(sql);
                return result > 0;
            }
        }

        public static void LoginLog(int userid, string name, int hosid)
        {
            using (var dao = TheseThreeDao.GetInstance())
            {
                var sql = string.Format("insert into login_log(userid,name,hospitalid,logintime) values({0},'{1}',{2},'{3}');", userid, name, hosid, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                dao.ExecuteCommand(sql);
            }
        }
    }
}