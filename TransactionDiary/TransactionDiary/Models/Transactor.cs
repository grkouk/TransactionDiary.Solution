namespace TransactionDiary.Models
{
   public class Transactor
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? Zip { get; set; }
        public string PhoneWork { get; set; }
        public string PhoneMobile { get; set; }
        public string PhoneFax { get; set; }
        public string EMail { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
