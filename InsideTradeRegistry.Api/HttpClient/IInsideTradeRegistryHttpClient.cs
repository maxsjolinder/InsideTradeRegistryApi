using System.Threading.Tasks;

namespace InsideTradeRegistry.Api.HttpClient
{
    internal interface IInsideTradeRegistryHttpClient
    {
        Task<byte[]> GetByteArrayAsync(string url);
    }
}
