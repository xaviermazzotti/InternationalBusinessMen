using InternationalBusinessMen.Data;
using InternationalBusinessMen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternationalBusinessMen.API.Models
{
    public class IndexModel
    {
        public string SearchString { get; set; }
        public int QuantityResult { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<Conversion> Conversions { get; set; }
        public decimal TotalInEURAmount { get; set; }
    }
}