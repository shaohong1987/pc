using System;
using System.Collections.Generic;
using System.Data;
using TheseThree.Admin.DataAccess;
using TheseThree.Admin.Models.Entities;
using TheseThree.Admin.Models.ViewModels;

namespace TheseThree.Admin.Models
{
    public class OrganizationModel
    {
        public Message GetOrganization(string name, OrganizationType organizationType, int hospitalid)
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
                            "select * from organization where otype=@ot and hospitalid=@hid and name like '%"+(string.IsNullOrEmpty(name) ? "" : name) +"%';",
                            new {ot = (int) organizationType, hid = hospitalid});
                    if (result != null && result.Rows.Count > 0)
                    {
                        List<Organization> organizations = new List<Organization>();
                        foreach (DataRow row in result.Rows)
                        {
                            var organization = new Organization
                            {
                                Id = Convert.ToInt32(row["id"]),
                                Name = Convert.ToString(row["name"]),
                                Count = Convert.ToInt32(row["count"]),
                                HospitalId = Convert.ToInt32(row["hospitalid"]),
                                OrganizationType =
                                    Convert.ToBoolean(row["otype"]) ? OrganizationType.Team : OrganizationType.Ward,
                                AdminName = DBNull.Value != row["adminname"] ? Convert.ToString(row["adminname"]) : ""
                            };
                            if (DBNull.Value != row["adminid"])
                                organization.AdminId = Convert.ToInt32(row["adminid"]);
                            else
                            {
                                organization.AdminId = -1;
                            }
                            organizations.Add(organization);
                        }

                        message.Status = MessageType.Success;
                        message.Msg = "查询成功";
                        message.Data = organizations;
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
        public Message UpdateOrganization(int id, string name, OrganizationType organizationType, int hospitalid)
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
                                "update organization set name=@n  where id=@oid and otype=@ot and hospitalid=@hid;",
                                new {n = name,oid=id, ot = (int) organizationType, hid = hospitalid});
                    }
                    else
                    {
                        result =
                            dao.ExecuteCommand("insert into organization(name,count,hospitalid,otype) values(@n,0,@hid,@ot);",
                                new {n = name, ot = (int) organizationType, hid = hospitalid});
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

        public bool CheckOrganizationExist(string name, OrganizationType organizationType, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    result =
                              dao.GetInt(
                                  "select count(*) from organization where name=@n and otype=@ot and hospitalid=@hid;",
                                  new { n = name, ot = (int)organizationType, hid = hospitalid })>0;
                }
            }
            catch (Exception)
            {
                result = true;
            }

            return result;
        }

        public bool DeleteOrg(string ids, OrganizationType organizationType, int hospitalid)
        {
            bool result;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    string sql = "delete from organization where id in ("+ids+") and otype="+((int)organizationType)+" and hospitalid="+ hospitalid + ";";
                    result = dao.ExecuteCommand(sql)>0;
                }
            }
            catch (Exception e)
            {
                result = true;
            }

            return result;
        }

        public List<CommonEntityViewModel> GetCommonAttr(int hospitalId)
        {
            List<CommonEntityViewModel> result=null;
            try
            {
                using (var dao = TheseThreeDao.GetInstance())
                {
                    var data =dao.GetDataTable("SELECT DISTINCT id as value,name,'ks' as type FROM `organization` WHERE hospitalid=@hid and otype=0 UNION SELECT DISTINCT id as value,name,'xz' as type FROM `organization` WHERE hospitalid=@hid and otype=1 UNION SELECT DISTINCT id as value,name,'zc' as type FROM zhichen WHERE hospitalcode=@hid UNION SELECT DISTINCT id as value,name,'gw' as type FROM gangwei WHERE hospitalcode=@hid UNION SELECT DISTINCT id as value,name,'lv' as type FROM level WHERE hospitalcode=@hid UNION SELECT DISTINCT id as value,name,'de' as type FROM degree WHERE hospitalcode=@hid; ", new { hid = hospitalId });
                    if (data != null && data.Rows.Count > 0)
                    {
                        result = new List<CommonEntityViewModel>();
                        foreach (DataRow row in data.Rows)
                        {
                            var model = new CommonEntityViewModel
                            {
                                Value = Convert.ToInt32(row["value"]),
                                Name = Convert.ToString(row["name"]),
                                Type = Convert.ToString(row["type"])
                            };
                            result.Add(model);
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        public static Message UpdateUserCount(int id,int hospitalId)
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
                            "update organization set count=(SELECT count(1) as qty FROM `user` where hospitalcode={1} and deptcode={2}) where id={0}",
                            id);
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