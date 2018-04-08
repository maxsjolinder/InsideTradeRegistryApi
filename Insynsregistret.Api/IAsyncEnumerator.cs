using System;
using System.Threading.Tasks;

namespace InsideTradeRegistry.Api
{

    public interface IAsyncEnumerator
    {
        object Current { get; }
    
        Task<bool> MoveNextAsync();

        Task ResetAsync();
    }

    public interface IAsyncEnumerator<T> : IAsyncEnumerator
    {
        new T Current { get; }
    }
}