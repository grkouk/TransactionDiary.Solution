namespace TransactionDiary.Models
{
    public class Facility
    {

        public int Id { get; set; }

        //[MaxLength(15)]
        public string Code { get; set; }

        //[MaxLength(200)]
        public string Name { get; set; }
        //[MaxLength(500)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }
    }
}
