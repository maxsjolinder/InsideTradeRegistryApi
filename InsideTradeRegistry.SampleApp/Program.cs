using InsideTradeRegistry.Api;
using System;

namespace InsideTradeRegistry.SampleApp
{
    class Program
    {
        static  void Main(string[] args)
        {           
            IInsideTradeRegistryApi api = new InsideTradeRegistryApi();

            var transactions = api.GetInsideTradeTransactionsAsync(new SearchQuery
            {
                Issuer = "Investor",
                PDMRPerson = "Johan Forssell",
                PublicationDateFrom = DateTime.Parse("2018-01-01")
            }).GetAwaiter().GetResult();
        }
    }
}
