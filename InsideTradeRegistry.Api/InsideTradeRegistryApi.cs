using InsideTradeRegistry.Api.HttpClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsideTradeRegistry.Api
{
    public class InsideTradeRegistryApi : IInsideTradeRegistryApi
    {
        private InsideTradeRegistryService insideTradeService;

        public InsideTradeRegistryApi()
        {
            insideTradeService = new InsideTradeRegistryService(new InsideTradeRegistryHttpClient());
        }

        public Task<IList<ITradeTransaction>> GetInsideTradeTransactionsAsync(SearchQuery searchQuery)
        {
            return insideTradeService.GetInsideTradeTransactionsAsync(searchQuery);
        }        
    }
}