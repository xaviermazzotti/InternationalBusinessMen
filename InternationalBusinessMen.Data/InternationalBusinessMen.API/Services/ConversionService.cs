using InternationalBusinessMen.API.Interfaces;
using InternationalBusinessMen.API.Utilities;
using InternationalBusinessMen.Data;
using InternationalBusinessMen.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace InternationalBusinessMen.API.Services
{
    public class ConversionService : IConversionService
    {
        private readonly DataModel _context;

        public ConversionService(DataModel context)
        {
            _context = context;
        }

        public List<Conversion> RetrieveSaveAllConversions()
        {
            try
            {
                var json = new WebClient().DownloadString("http://quiet-stone-2094.herokuapp.com/rates.json");
                var newConversions = JsonConvert.DeserializeObject<Conversion[]>(json).ToList();

                if (newConversions.Any())
                {
                    if(_context.Conversions.Any())
                        _context.Conversions.RemoveRange(_context.Conversions);

                    _context.Conversions.AddRange(newConversions);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetConversions", ex);
            }

            return _context.Conversions.ToList();
        }

        //TRYING RECURSION

        //public IEnumerable<Transaction> ConvertToEUR(IEnumerable<Transaction> transactions)
        //{
        //    Conversion ConversionToUse = new Conversion();

        //    var conversions = _context.Conversions.ToList();
        //    var transactionsByCurrency = transactions.GroupBy(e => e.Currency).ToList();

        //    try
        //    {
        //        foreach (var group in transactionsByCurrency)
        //        {
        //            var fromCurrency = group.Key;
        //            decimal finalRate;
        //            if (fromCurrency != "EUR")
        //            {
        //                finalRate = GetConversionRate(conversions, fromCurrency, "EUR");
        //            }
        //            else
        //            {
        //                finalRate = 1M;
        //            }

        //            foreach (var item in group)
        //            {
        //                item.EURAmount = Math.Round(item.Amount * finalRate, 2);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("ConvertToEUR", ex);
        //    }

        //    return transactions;
        //}

        //private decimal GetConversionRate(IEnumerable<Conversion> conversions, string fromCurrency, string toCurrency)
        //{
        //    var matchingConversions = conversions.Where(c => c.From == fromCurrency && c.To == toCurrency);

        //    if (matchingConversions.Any())
        //        return matchingConversions.First().Rate;

        //    //All conversions where currency is final
        //    var nextLevelConversions = conversions.Where(c => c.To == fromCurrency);

        //    foreach(var conversion in nextLevelConversions)
        //    {
        //        return GetConversionRate(conversions.Where(c => c.From == conversion.From), conversion.From, toCurrency) / conversion.Rate;
        //    }

        //    return 0;
        //}

        public IEnumerable<Transaction> ConvertToEUR(IEnumerable<Transaction> transactions)
        {
            Conversion ConversionToUse = new Conversion();

            var conversions = _context.Conversions.ToList();
            var transactionsByCurrency = transactions.GroupBy(e => e.Currency).ToList();
            
            var directConv = conversions.Where(e => e.To == "EUR");
            var indirectConv = conversions.Where(e => e.To != "EUR" && e.From != "EUR");

            try
            {
                foreach (var list in transactionsByCurrency) 
                {
                    if (list.Key != "EUR")
                    {
                        if (directConv.Any(e => e.From == list.Key)) //One step conversion
                        {
                            decimal rate = directConv.FirstOrDefault(e => e.From == list.First().Currency).Rate;
                            foreach (var item in list)
                            {
                                item.EURAmount = Math.Round(item.Amount * rate, 2);
                            }
                        }
                        else
                        {
                            //Two step conversion
                            Conversion ConvToEUR = new Conversion();
                            Conversion PreConv = new Conversion();
                            bool found = false;

                            foreach (var ind in indirectConv)
                            {
                                if (ind.From == list.Key)
                                {
                                    if (directConv.Any(e => e.From == ind.To) && found != true)
                                    {
                                        ConvToEUR = directConv.Where(e => e.From == ind.To).First();
                                        PreConv = ind;
                                        found = true; 
                                    }
                                }
                            }

                            if (found) //Confirmed two step conversion
                            {
                                foreach (var item in list)
                                {
                                    decimal firstResult = item.Amount * PreConv.Rate;
                                    decimal finalResult = firstResult * ConvToEUR.Rate;
                                    item.EURAmount = Math.Round(finalResult, 2);
                                }
                            }
                            else //Three step conversion
                            {
                                Conversion ThirdPreConv = new Conversion();

                                foreach (var ind in indirectConv)
                                {
                                    if (directConv.Any(e => e.From == ind.To) && found != true)
                                    {
                                        ConvToEUR = directConv.Where(e => e.From == ind.To).First();
                                        PreConv = ind;

                                        foreach (var indi in indirectConv)
                                        {
                                            if (indirectConv.Any(e => e.To == PreConv.From && e.From == list.Key) && found != true)
                                            {
                                                ThirdPreConv = indirectConv.Where(e => e.To == PreConv.From && e.From == list.Key).First();
                                                found = true;
                                            }
                                        }
                                    }
                                }

                                if (found) //Confirmed three step conversion
                                {
                                    foreach (var item in list)
                                    {
                                        decimal firstResult = item.Amount * ThirdPreConv.Rate;
                                        decimal secondResult = firstResult * PreConv.Rate;
                                        decimal finalResult = secondResult * ConvToEUR.Rate;
                                        item.EURAmount = Math.Round(finalResult, 2);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            item.EURAmount = Math.Round(item.Amount, 2);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error("ConvertToEUR", ex);
            }

            return transactions;
        }
    }
}