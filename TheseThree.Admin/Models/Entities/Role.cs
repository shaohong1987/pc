using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheseThree.Admin.Models.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public string RoleDesc { get; set; }

        public int HospitalId { get; set; }
    }
}