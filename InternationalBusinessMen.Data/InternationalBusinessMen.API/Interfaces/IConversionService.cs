using InternationalBusinessMen.Data;
using InternationalBusinessMen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternationalBusinessMen.API.Interfaces
{
    public interface IConversionService
    {
        IEnumerable<Transaction> ConvertToEUR(IEnumerable<Transaction> transactions);
        List<Conversion> RetrieveSaveAllConversions();
    }
}
