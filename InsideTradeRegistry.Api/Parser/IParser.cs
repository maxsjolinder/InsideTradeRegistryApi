using System.Collections.Generic;

namespace InsideTradeRegistry.Api.Parser
{
    internal interface IParser
    {
        IList<IList<string>> ParseData(string data);
    }
}
