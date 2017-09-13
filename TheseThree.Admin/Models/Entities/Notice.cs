namespace TheseThree.Admin.Models.Entities
{
    public class Notice
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public int Educode { get; set; }
        public int Testcode { get; set; }
        public string Sendtime { get; set; }
        public int Hospitalcode { get; set; }
        public int Groupid { get; set; }
        public int Isvalued { get; set; } 

        public string GroupName { get; set; }
    }
}