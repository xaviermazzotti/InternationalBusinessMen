using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternationalBusinessMen.Data.Model
{
    public class Conversion
    {
        [Key]
        public int ConversionId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal Rate { get; set; }
    }
}
