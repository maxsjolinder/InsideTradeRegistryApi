using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsideTradeRegistry.Api
{
    public interface IInsideTradeRegistryApi
    {
        Task<IList<ITradeTransaction>> GetInsideTradeTransactionsAsync(SearchQuery searchProperties);
    }
}