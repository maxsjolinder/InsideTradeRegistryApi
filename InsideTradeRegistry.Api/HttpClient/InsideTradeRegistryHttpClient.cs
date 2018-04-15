using System.Threading.Tasks;

namespace InsideTradeRegistry.Api.HttpClient
{
    internal class InsideTradeRegistryHttpClient : IInsideTradeRegistryHttpClient
    {
        private static readonly System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

        public Task<byte[]> GetByteArrayAsync(string url)
        {
            return httpClient.GetByteArrayAsync(url);
        }
    }
}
