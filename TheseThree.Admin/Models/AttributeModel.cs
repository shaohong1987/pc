using System;
using System.Collections.Generic;
using System.Data;
using TheseThree.Admin.DataAccess;
using TheseThree.Admin.Models.Entities;

namespace TheseThree.Admin.Models
{
    public class AttributeModel
    {
        public Message GetZC(string name, int hospitalid)
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
                    var result =
                        dao.GetDataTable(
                            "select * from zhichen where  Hospitalcode=@hid and name like '%" +
                            (string.IsNullOrEmpty(name) ? "" : name) + "%';",
                            new { hid = hospitalid });
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<ZhiChen> zhichens = new List<ZhiChen>();
                        foreach (DataRow row in result.Rows)
                        {
                            var zhichen = new ZhiChen
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Name = Convert.ToString(row["name"]),
                                Hospitalcode = Convert.ToInt32(row["Hospitalcode"])
                            };
                            zhichen.Desc = DBNull.Value != row["Desc"]
                                ? Convert.ToString(row["Desc"])
                                : "";

                            zhichens.Add(zhichen);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = zhichens;
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

        public Message UpdateZC(int id, string name, int hospitalid)
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
                    int result;
                    if (id > 0)
                    {
                        result =
                            dao.ExecuteCommand(
                                "update ZhiChen set name=@n  where id=@oid  and Hospitalcode=@hid;",
                                new { n = name, oid = id, hid = hospitalid });
                    }
                    else
                    {
                        result =
                            dao.ExecuteCommand(
                                "insert into ZhiChen(name,hospitalcode) values(@n,@hid);",
                                new { n = name, hid = hospitalid });
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

        public bool CheckZCExist(string name, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    result =
                        dao.GetInt(
                            "select count(*) from ZhiChen where name=@n and Hospitalcode=@hid;",
                            new { n = name, hid = hospitalid }) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public bool DeleteZC(string ids, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "delete from ZhiChen where id in (" + ids + ")  and hospitalcode=" + hospitalid + ";";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public Message GetGW(string name, int hospitalid)
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
                    var result =
                        dao.GetDataTable(
                            "select * from gangwei where  Hospitalcode=@hid and name like '%" +
                            (string.IsNullOrEmpty(name) ? "" : name) + "%';",
                            new { hid = hospitalid });
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<GangWei> gangweis = new List<GangWei>();
                        foreach (DataRow row in result.Rows)
                        {
                            var gangWei = new GangWei()
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Name = Convert.ToString(row["name"]),
                                Hospitalcode = Convert.ToInt32(row["Hospitalcode"])
                            };
                            gangWei.Desc = DBNull.Value != row["Desc"]
                                ? Convert.ToString(row["Desc"])
                                : "";

                            gangweis.Add(gangWei);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = gangweis;
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

        public Message UpdateGW(int id, string name, int hospitalid)
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
                    int result;
                    if (id > 0)
                    {
                        result =
                            dao.ExecuteCommand(
                                "update GangWei set name=@n  where id=@oid  and Hospitalcode=@hid;",
                                new { n = name, oid = id, hid = hospitalid });
                    }
                    else
                    {
                        result =
                            dao.ExecuteCommand(
                                "insert into GangWei(name,HospitalcodE) values(@n,@hid);",
                                new { n = name, hid = hospitalid });
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

        public bool CheckGWExist(string name, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    result =
                        dao.GetInt(
                            "select count(*) from GangWei where name=@n and Hospitalcode=@hid;",
                            new { n = name, hid = hospitalid }) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public bool DeleteGW(string ids, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "delete from GangWei where id in (" + ids + ")  and Hospitalcode=" + hospitalid + ";";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }


        public Message GetLevel(string name, int hospitalid)
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
                    var result =
                        dao.GetDataTable(
                            "select * from level where  Hospitalcode=@hid and name like '%" +
                            (string.IsNullOrEmpty(name) ? "" : name) + "%';",
                            new { hid = hospitalid });
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<Level> levels = new List<Level>();
                        foreach (DataRow row in result.Rows)
                        {
                            var level = new Level()
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Name = Convert.ToString(row["name"]),
                                Hospitalcode = Convert.ToInt32(row["Hospitalcode"])
                            };
                            level.Desc = DBNull.Value != row["Desc"]
                                ? Convert.ToString(row["Desc"])
                                : "";

                            levels.Add(level);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = levels;
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

        public Message UpdateLevel(int id, string name, int hospitalid)
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
                    int result;
                    if (id > 0)
                    {
                        result =
                            dao.ExecuteCommand(
                                "update level set name=@n  where id=@oid  and Hospitalcode=@hid;",
                                new { n = name, oid = id, hid = hospitalid });
                    }
                    else
                    {
                        result =
                            dao.ExecuteCommand(
                                "insert into level(name,HospitalcodE) values(@n,@hid);",
                                new { n = name, hid = hospitalid });
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

        public bool CheckLevelExist(string name, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    result =
                        dao.GetInt(
                            "select count(*) from level where name=@n and Hospitalcode=@hid;",
                            new { n = name, hid = hospitalid }) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public bool DeleteLevel(string ids, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "delete from level where id in (" + ids + ")  and Hospitalcode=" + hospitalid + ";";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public Message GetDegree(string name, int hospitalid)
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
                    var result =
                        dao.GetDataTable(
                            "select * from degree where  Hospitalcode=@hid and name like '%" +
                            (string.IsNullOrEmpty(name) ? "" : name) + "%';",
                            new { hid = hospitalid });
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<Degree> degrees = new List<Degree>();
                        foreach (DataRow row in result.Rows)
                        {
                            var degree = new Degree()
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Name = Convert.ToString(row["name"]),
                                Hospitalcode = Convert.ToInt32(row["Hospitalcode"])
                            };
                            degree.Desc = DBNull.Value != row["Desc"]
                                ? Convert.ToString(row["Desc"])
                                : "";

                            degrees.Add(degree);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = degrees;
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

        public Message UpdateDegree(int id, string name, int hospitalid)
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
                    int result;
                    if (id > 0)
                    {
                        result =
                            dao.ExecuteCommand(
                                "update Degree set name=@n  where id=@oid  and Hospitalcode=@hid;",
                                new { n = name, oid = id, hid = hospitalid });
                    }
                    else
                    {
                        result =
                            dao.ExecuteCommand(
                                "insert into Degree(name,HospitalcodE) values(@n,@hid);",
                                new { n = name, hid = hospitalid });
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

        public bool CheckDegreeExist(string name, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    result =
                        dao.GetInt(
                            "select count(*) from Degree where name=@n and Hospitalcode=@hid;",
                            new { n = name, hid = hospitalid }) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public bool DeleteDegree(string ids, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "delete from Degree where id in (" + ids + ")  and Hospitalcode=" + hospitalid + ";";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public Message GetRoles()
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
                    var result =
                        dao.GetDataTable(
                            "select * from admin_role");
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<Role> roles = new List<Role>();
                        foreach (DataRow row in result.Rows)
                        {
                            var role = new Role
                            {
                                Id = Convert.ToInt32(row["id"]),
                                RoleName = Convert.ToString(row["rolename"]),
                                RoleDesc = Convert.ToString(row["roledesc"]),
                                HospitalId = Convert.ToInt32(row["hospitalid"])
                            };
                            roles.Add(role);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = roles;
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

        public Message GetAdv(int hosid)
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
                    var result =
                        dao.GetDataTable(
                            "select * from adv where hosid = @hosid", new { hosid = hosid });
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<Adv> advs = new List<Adv>();
                        foreach (DataRow row in result.Rows)
                        {
                            var role = new Adv
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Title = Convert.ToString(row["title"]),
                                Url = Convert.ToString(row["Url"]),
                                HosId = Convert.ToInt32(row["HosId"])
                            };
                            advs.Add(role);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = advs;
                    }
                }
            }
            catch (Exception e)
            {
                message.Status = MessageType.Error;
                message.Msg = "出错了";
            }

            return message;
        }

        public static bool DelAdv(int id)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "delete from adv where  id=" + id;
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static bool AddAdv(string title, string url, int hosid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var sql = "insert into adv(title,url,hosid) values('" + title + "','" + url + "'," + hosid + ");";
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public static Message GetRole(int id)
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
                    var result =
                        dao.GetDataTable(
                            "select * from admin_role where id = @id", new { id = id });
                    if (result != null && result.Rows.Count > 0)
                    {
                        var row = result.Rows[0];
                        var role = new Role
                        {
                            Id = Convert.ToInt32(row["id"]),
                            RoleName = Convert.ToString(row["rolename"]),
                            RoleDesc = Convert.ToString(row["roledesc"]),
                            HospitalId = Convert.ToInt32(row["hospitalid"])
                        };
                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = role;
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
        public static Message GetUserForRole(int roleid, string loginId, int hospitalid)
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
                    var sql = "select * from admin_user where usertype =" + roleid + " and hospitalid =" + hospitalid + "";
                    if (!string.IsNullOrEmpty(loginId))
                    {
                        sql += "  and username like '%" + loginId + "%'  ";
                    }

                    var result =
                        dao.GetDataTable(sql);
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<RoleUser> RoleUsers = new List<RoleUser>();
                        foreach (DataRow row in result.Rows)
                        {
                            var RoleUser = new RoleUser
                            {
                                Id = Convert.ToInt32(row["id"]),
                                loginID = Convert.ToString(row["username"]),
                                UserName = Convert.ToString(row["name"]),
                                HospitalId = Convert.ToInt32(row["hospitalid"])
                            };
                            RoleUsers.Add(RoleUser);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = RoleUsers;
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
        public static Boolean DeleteRole(int id)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql;
                    sql = "delete from admin_user where id=" + id;
                    result = dao.ExecuteCommand(sql) > 0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }
        public static Message SaveRoleUser(string ids, int roleid, int hospitalId)
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
                                    "insert into admin_user(username,userpwd,hospitalid,usertype,state,name,deptcode) values((select phone from user where id = {0}),{1},{2},{3},{4},(select name from user where id = {5}),(select deptcode from user where id = {6}));",
                                    item, 0, hospitalId, roleid, 1, item, item);
                        }
                    }
                    else
                    {
                        sql +=
                                string.Format(
                                    "insert into admin_user(username,userpwd,hospitalid,usertype,state,name,deptcode) values((select phone from user where id = {0}),{1},{2},{3},{4},(select name from user where id = {5}),(select deptcode from user where id = {6}));",
                                    ids, 0, hospitalId, roleid, 1, ids, ids);
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
    }
}