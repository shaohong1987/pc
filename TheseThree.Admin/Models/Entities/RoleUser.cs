using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheseThree.Admin.Models.Entities
{
    public class RoleUser
    {
        public int Id { get; set; }

        public string LoginId { get; set; }

        public string UserName { get; set; }

        public int HospitalId { get; set; }

        public string Depart { get; set; }
        public string Gw { get; set; }
    }
}