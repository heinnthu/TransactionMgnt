using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionMgnt
{

    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        [StringLength(5)]
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        [StringLength(10)]
        public string Status { get; set; }
        [StringLength(50)]
        public string CreatedIP { get; set; }
        [StringLength(50)]
        public string CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }

    }

}
