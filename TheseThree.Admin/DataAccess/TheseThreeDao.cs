using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MySqlSugar;

namespace TheseThree.Admin.DataAccess
{
    public class TheseThreeDao
    {
        public static string ConnectionString
        {
            get
            {
                var reval = ConfigurationManager.ConnectionStrings["ehospital"].ConnectionString;
                return reval;
            }
        }
        public static SqlSugarClient GetInstance()
        {
            var db = new SqlSugarClient(ConnectionString);
            return db;
        }
    }
}