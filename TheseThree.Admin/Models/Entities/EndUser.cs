namespace TheseThree.Admin.Models.Entities
{
    public class EndUser
    {
        public int Id { get; set; }
        public string LoginId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Token { get; set; }
        public int Hospitalcode { get; set; }
        public string Hospitalname { get; set; }
        public int Wardcode { get; set; }
        public string Wardname { get; set; }
        public int Deptcode { get; set; }
        public string Deptname { get; set; }
        public string Card { get; set; }
        public int Gwcode { get; set; }
        public string Gwname { get; set; }
        public int Zccode { get; set; }
        public string Zcname { get; set; }
        public int Lvcode { get; set; }
        public string Lvname { get; set; }
        public int Decode { get; set; }
        public string Dename { get; set; }
        public string Xzcode { get; set; }
        public string Xzname { get; set; }
        public int Isvalued { get; set; } //0:正常，1：失效
        public string Iostoken { get; set; }
        public int Type { get; set; } //标识0:ios,1:android
    }
}