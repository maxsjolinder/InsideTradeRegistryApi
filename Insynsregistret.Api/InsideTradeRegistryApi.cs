using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace InsideTradeRegistry.Api
{
    public class InsideTradeRegistryApi : IInsideTradeRegistryApi
    {
        InsideTradeRegistryHttpClient insideTradeHttpClient;
        public InsideTradeRegistryApi()
        {
            insideTradeHttpClient = new InsideTradeRegistryHttpClient();
        }

        public Task<IList<ITradeTransaction>> GetInsideTradeTransactionsAsync(SearchQuery searchQuery)
        {
            return insideTradeHttpClient.GetInsideTradeTransactionsAsync(searchQuery);
        }        
    }
}