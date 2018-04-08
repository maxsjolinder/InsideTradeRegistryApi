using InsideTradeRegistry.Api;
using System.Linq;

namespace InsideTradeRegistry.SampleApp
{
    class Program
    {
        static  void Main(string[] args)
        {           
            IInsideTradeRegistryApi api = new InsideTradeRegistryApi();

            var transactions = api.GetInsideTradeTransactionsAsync(new SearchQuery ( issuer:"Investor", person: "Johan Forssell")).GetAwaiter().GetResult();
        }
    }
}
