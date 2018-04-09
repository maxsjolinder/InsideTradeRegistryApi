using System;
using System.Globalization;

namespace InsideTradeRegistry.Api
{
    public class TradeTransaction : ITradeTransaction
    {
        [DataColumn(Name = "Publication date", CultureToUse = "en-GB")]
        public DateTime PublicationDate {get;set;}

        [DataColumn(Name = "Issuer")]
        public string Issuer {get;set;}

        [DataColumn(Name = "LEI-code")]
        public string LEICode { get; set; }

        [DataColumn(Name = "Notifier")]
        public string Notifier { get; set; }
        
        [DataColumn(Name = "Person discharging managerial responsibilities")]
        public string Person { get; set; }

        [DataColumn(Name = "Position")]
        public string Position { get; set; }

        [DataColumn(Name = "Closely associated")]
        public string CloselyAssociated { get; set; }

        [DataColumn(Name = "Amendment")]
        public string Amendment { get; set; }

        [DataColumn(Name = "Details of amendment")]
        public string DetailsOfAmendment { get; set; }

        [DataColumn(Name = "Initial notification")]
        public string InitialNotification { get; set; }

        [DataColumn(Name = "Linked to share option programme")]
        public string PartOfShareOptionProgramme { get; set; }

        [DataColumn(Name = "Nature of transaction")]
        public string NatureOfTransaction { get; set; }

        [DataColumn(Name = "Instrument")]
        public string Instrument { get; set; }

        [DataColumn(Name = "ISIN")]
        public string ISIN { get; set; }

        [DataColumn(Name = "Transaction date")]
        public string TransactionDate { get; set; }

        [DataColumn(Name = "Volume")]
        public int Volume { get; set; }

        [DataColumn(Name = "Unit")]
        public string Unit { get; set; }

        [DataColumn(Name = "Price", CultureToUse = "en-GB")]
        public double Price { get; set; }

        [DataColumn(Name = "Currency")]
        public string Currency { get; set; }

        [DataColumn(Name = "Trading venue")]
        public string TradingVenue { get; set; }

        [DataColumn(Name = "Status")]
        public string Status { get; set; }
    }
}