using InternationalBusinessMen.API.Interfaces;
using InternationalBusinessMen.API.Models;
using InternationalBusinessMen.API.Utilities;
using InternationalBusinessMen.Data;
using InternationalBusinessMen.Data.Model;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace InternationalBusinessMen.API.Controllers
{
    public class HomeController : Controller
    {
        public readonly DataModel _context;
        public readonly ITransactionService _transactionService;
        public readonly IConversionService _conversionService;

        public HomeController(ITransactionService transactionService, IConversionService conversionService)
        {
            _transactionService = transactionService;
            _conversionService = conversionService;
        }

        public async Task<ActionResult> Index([FromBody]IndexModel model, string value)
        {
            ViewBag.Title = "Transacciones";

            try
            {
                if (!string.IsNullOrWhiteSpace(model.SearchString)) //Searching by SKU
                {
                    model.Transactions = GetTransactionsBySkuFromApi(model.SearchString);
                    model.TotalInEURAmount = Math.Round(model.Transactions.Sum(e => e.EURAmount), 2);
                }
                else //Empty SKU searching
                {
                    model.Transactions = GetTransactionsFromApi();
                    model.Conversions = GetConversionsFromApi();
                }
            }
            catch(Exception ex)
            {
                Log.Error("HomeIndex", ex);
            }

            model.QuantityResult = model.Transactions.Count();

            return View(model);
        }

        public List<Conversion> GetConversionsFromApi()
        {
            HttpClient client = new HttpClient();
            List<Conversion> conversions = new List<Conversion>();

            var responseMessage = client.GetAsync(HttpContext.Request.Url + "api/external/conversions").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                conversions = JsonConvert.DeserializeObject<List<Conversion>>(responseData);
            }

            return conversions;
        }

        public List<Transaction> GetTransactionsFromApi()
        {
            HttpClient client = new HttpClient();
            List<Transaction> transactions = new List<Transaction>();

            var responseMessage = client.GetAsync(HttpContext.Request.Url + "api/external/transactions").Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                transactions = JsonConvert.DeserializeObject<List<Transaction>>(responseData);
            }

            return transactions;
        }

        public List<Transaction> GetTransactionsBySkuFromApi(string searchSku)
        {
            HttpClient client = new HttpClient();
            List<Transaction> transactions = new List<Transaction>();

            var responseMessage = client.GetAsync(HttpContext.Request.Url + "api/external/transactions/" + searchSku).Result;

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                transactions = JsonConvert.DeserializeObject<List<Transaction>>(responseData);
            }

            return transactions;
        }
    }
}
