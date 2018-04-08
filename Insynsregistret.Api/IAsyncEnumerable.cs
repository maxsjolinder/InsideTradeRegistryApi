using System;
using System.Threading.Tasks;

namespace InsideTradeRegistry.Api
{
    public interface IAsyncEnumerable<T>
    {
        Task<IAsyncEnumerator<T>> GetAsyncEnumeratorAsync();
    }
}