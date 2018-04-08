using System;

namespace InsideTradeRegistry.Api
{
    public class SearchQuery
    {
        public SearchQuery()
        {
            PerformFullSearch = true;
        }
        public SearchQuery(string issuer = "", string person = "")
        {
            Issuer = issuer;
            Person = person;
            PerformFullSearch = false;
        }

        internal bool PerformFullSearch { get; private set; }

        public string Issuer { get; private set; }

        public string Person { get; private set; }
    }
}