using System.Collections.Generic;

namespace TheseThree.Admin.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int HospitalId { get; set; }

        public string HospitalName { get; set; }

        public List<string> UserTypes { get; set; }

        public string HospitalRegDate { get; set; }

        public string HospitalEndDate { get; set; }
    }
}