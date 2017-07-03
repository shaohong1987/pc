using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheseThree.Admin.Models.Entities
{
    public class Organization
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AdminId { get; set; }

        public string AdminName { get; set; }

        public int Count { get; set; }

        public int HospitalId { get; set; }

        public OrganizationType OrganizationType { get; set; }
    }
}