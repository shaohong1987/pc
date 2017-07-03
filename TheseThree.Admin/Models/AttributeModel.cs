using System;
using System.Collections.Generic;
using System.Data;
using TheseThree.Admin.DataAccess;
using TheseThree.Admin.Models.Entities;

namespace TheseThree.Admin.Models
{
    public class AttributeModel
    {
        public Message GetZC(string name,int hospitalid)
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
                            new {hid = hospitalid});
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

        public Message UpdateZC(int id, string name,  int hospitalid)
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
                                new {n = name, oid = id, hid = hospitalid});
                    }
                    else
                    {
                        result =
                            dao.ExecuteCommand(
                                "insert into ZhiChen(name,hospitalcode) values(@n,@hid);",
                                new {n = name, hid = hospitalid});
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
                            new {n = name, hid = hospitalid}) > 0;
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

    }
}