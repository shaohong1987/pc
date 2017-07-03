namespace TheseThree.Admin.Models.Entities
{
    public class Paper
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public float TotalCent { get; set; }
        public int Duration { get; set; }
        public int DeptCode { get; set; }
        public string DeptName { get; set; }
        public int HospitalId { get; set; }
        public bool State { get; set; }

    }
}