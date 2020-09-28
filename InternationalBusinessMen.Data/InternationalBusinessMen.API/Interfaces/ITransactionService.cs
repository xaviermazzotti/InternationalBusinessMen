using InternationalBusinessMen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternationalBusinessMen.API.Interfaces
{
    public interface ITransactionService
    {
        List<Transaction> GetTransactionsBySku(string sku);
        List<Transaction> RetrieveSaveAllTransactions();
    }
}




