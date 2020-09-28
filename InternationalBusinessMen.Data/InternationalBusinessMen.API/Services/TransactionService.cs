using InternationalBusinessMen.API.Interfaces;
using InternationalBusinessMen.API.Utilities;
using InternationalBusinessMen.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace InternationalBusinessMen.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DataModel _context;
        private readonly IConversionService _conversionService;

        public TransactionService(DataModel context, IConversionService conversionService)
        {
            _context = context;
            _conversionService = conversionService;
        }

        
        public List<Transaction> GetTransactionsBySku(string sku)
        {
            var filteredTransactions = _context.Transactions.Where(e => e.Sku.ToUpper() == sku.ToUpper());
            if (filteredTransactions.Any())
            {
                return _conversionService.ConvertToEUR(filteredTransactions).ToList();
            }

            return new List<Transaction>();
        }

        public List<Transaction> RetrieveSaveAllTransactions()
        {
            try
            {
                var json = new WebClient().DownloadString("http://quiet-stone-2094.herokuapp.com/transactions.json");
                var newTransactions = JsonConvert.DeserializeObject<Transaction[]>(json).ToList();

                if (newTransactions.Any())
                {
                    if(_context.Transactions.Any())
                        _context.Transactions.RemoveRange(_context.Transactions);

                    _context.Transactions.AddRange(newTransactions);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetTransacions", ex);
            }

            return _context.Transactions.ToList();

        }
    }
}