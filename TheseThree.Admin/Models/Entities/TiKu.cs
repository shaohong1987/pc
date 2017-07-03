namespace TheseThree.Admin.Models.Entities
{
    public class TiKu
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public string Remark { get; set; }

        public int HospitalId { get; set; }
    }
}