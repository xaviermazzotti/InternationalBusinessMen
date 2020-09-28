using InternationalBusinessMen.API.Interfaces;
using InternationalBusinessMen.API.Utilities;
using InternationalBusinessMen.Data;
using InternationalBusinessMen.Data.Model;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Util;

namespace InternationalBusinessMen.API.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {
        private readonly DataModel _context;
        private readonly IConversionService _conversionService;
        private readonly ITransactionService _transactionService;

        public ApiController(DataModel context, IConversionService conversionService, ITransactionService transactionService)
        {
            _context = context;
            _conversionService = conversionService;
            _transactionService = transactionService;
        }

        [Route("api/external/conversions")]
        [HttpGet]
        public List<Conversion> GetConversions()
        {
            return _conversionService.RetrieveSaveAllConversions();
        }

        [Route("api/external/transactions")]
        [HttpGet]
        public List<Transaction> GetTransacions()
        {

            return _transactionService.RetrieveSaveAllTransactions();
        }

        [Route("api/external/transactions/{sku}")]
        [HttpGet]
        public List<Transaction> GetTransacionsBySku(string sku)
        {
            return _transactionService.GetTransactionsBySku(sku);
        }
    }
}
