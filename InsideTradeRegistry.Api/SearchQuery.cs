using System;

namespace InsideTradeRegistry.Api
{
    public class SearchQuery
    {
        public string Issuer { get;  set; }

        public string PDMRPerson { get; set; }

        public DateTime TransactionDateFrom { get; set; }

        public DateTime TransactionDateTo { get; set; }

        public DateTime PublicationDateFrom { get; set; }
                        
        public DateTime PublicationDateTo { get; set; }
    }
}