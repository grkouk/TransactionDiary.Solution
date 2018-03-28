using System;

namespace TransactionDiary.Models
{
    /// <summary>
    /// Class that is used in all data filters
    /// </summary>
    public class DateFilter
    {
        public string Name { get; set; }
        public string SystemCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
