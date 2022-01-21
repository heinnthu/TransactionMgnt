using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionMgnt
{

    public class TransactionDto
    {    
        public string TransactionId { get; set; }
        public string Amount { get; set; }     
        public string CurrencyCode { get; set; }
        public string TransactionDate { get; set; }      
        public string Status { get; set; }      

    }

}
