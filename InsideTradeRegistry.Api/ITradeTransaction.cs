using System;

namespace InsideTradeRegistry.Api
{
    public interface ITradeTransaction
    {
        DateTime PublicationDate { get; set; }
        
        string Issuer { get; set; }
        
        string LEICode { get; set; }
        
        string Notifier { get; set; }
        
        string Person { get; set; }
        
        string Position { get; set; }
        
        bool CloselyAssociated { get; set; }
        
        bool Amendment { get; set; }
        
        string DetailsOfAmendment { get; set; }
        
        bool InitialNotification { get; set; }
        
        bool PartOfShareOptionProgramme { get; set; }
        
        string NatureOfTransaction { get; set; }
        
        string InstrumentName { get; set; }

        string InstrumentType { get; set; }

        string ISIN { get; set; }

        DateTime TransactionDate { get; set; }
        
        double Volume { get; set; }
        
        string Unit { get; set; }
        
        double Price { get; set; }
        
        string Currency { get; set; }
        
        string TradingVenue { get; set; }
        
        string Status { get; set; }
    }
}
