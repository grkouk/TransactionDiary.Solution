using System;

namespace TransactionDiary.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        /// <summary>
        /// Ημερομηνία Κίνησης
        /// </summary>
        
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// Αριθμός Παραστατικού
        /// </summary>
        public string ReferenceCode { get; set; }
        
        public int TransactorId { get; set; }

        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public int CostCentreId { get; set; }
        public int RevenueCentreId { get; set; }

        public string Description { get; set; }
        /// <summary>
        /// Ποσό ΦΠΑ
        /// </summary>
        public decimal AmountFpa { get; set; }
        /// <summary>
        /// Καθαρό Ποσό
        /// </summary>
        public decimal AmountNet { get; set; }

        public decimal AmountTotal { get; set; }
        public int Kind { get; set; }

    }
}
