using System;

namespace InsideTradeRegistry.Api
{
    public class TradeTransaction : ITradeTransaction
    {
        [DataColumn(Name = "Publication date", CultureToUse = "en-GB")]
        public DateTime PublicationDate { get; set; }

        [DataColumn(Name = "Issuer")]
        public string Issuer { get; set; }

        [DataColumn(Name = "LEI-code")]
        public string LEICode { get; set; }

        [DataColumn(Name = "Notifier")]
        public string Notifier { get; set; }

        [DataColumn(Name = "Person discharging managerial responsibilities")]
        public string Person { get; set; }

        [DataColumn(Name = "Position")]
        public string Position { get; set; }

        [DataColumnBoolean(Name = "Closely associated", TrueStrings = new string[] { "Yes", "True"}, FalseStrings = new string[] { "" })]
        public bool CloselyAssociated { get; set; }

        [DataColumnBoolean(Name = "Amendment", TrueStrings = new string[] { "Yes", "True" }, FalseStrings = new string[] { "" })]
        public bool Amendment { get; set; }

        [DataColumn(Name = "Details of amendment")]
        public string DetailsOfAmendment { get; set; }

        [DataColumnBoolean(Name = "Initial notification", TrueStrings = new string[] { "Yes", "True" }, FalseStrings = new string[] { "" })]
        public bool InitialNotification { get; set; }

        [DataColumnBoolean(Name = "Linked to share option programme", TrueStrings = new string[] { "Yes", "True" }, FalseStrings = new string[] { "" })]
        public bool PartOfShareOptionProgramme { get; set; }

        [DataColumn(Name = "Nature of transaction")]
        public string NatureOfTransaction { get; set; }

        [DataColumn(Name = "Instrument name")]
        public string InstrumentName { get; set; }

        // Note: The column is misspelled by Finansinspektionen
        [DataColumn(Name = "Intrument type")]
        public string InstrumentType { get; set; }

        [DataColumn(Name = "ISIN")]
        public string ISIN { get; set; }

        [DataColumn(Name = "Transaction date", CultureToUse = "en-GB")]
        public DateTime TransactionDate { get; set; }

        [DataColumn(Name = "Volume", CultureToUse = "en-GB")]
        public double Volume { get; set; }

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