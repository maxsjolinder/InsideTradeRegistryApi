using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insynsregistret.Api
{
    public interface IInsynsregistretApi
    {
        Task<IEnumerable<string>> GetAllEntitiesAsync();
    }
}