using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheseThree.Admin.Models.Entities
{
    public class ExamAllInfo
    {
        public int TestId { get; set; }

        public string ExamName { get; set; }

        public string ExamStyle { get; set; }

        public string ExamTime { get; set; }

        public int shouldCome { get; set; }

        public int realCome { get; set; }

        public int unCome { get; set; }

        public int jigeScore { get; set; }
    }
}