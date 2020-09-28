using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Data
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public string Sku { get; set; }
        public decimal Amount { get; set; }
        public decimal EURAmount { get; set; }
        public string Currency { get; set; }
    }
}
