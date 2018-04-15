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
        
        string CloselyAssociated { get; set; }
        
        string Amendment { get; set; }
        
        string DetailsOfAmendment { get; set; }
        
        string InitialNotification { get; set; }
        
        string PartOfShareOptionProgramme { get; set; }
        
        string NatureOfTransaction { get; set; }
        
        string Instrument { get; set; }
        
        string ISIN { get; set; }

        DateTime TransactionDate { get; set; }
        
        int Volume { get; set; }
        
        string Unit { get; set; }
        
        double Price { get; set; }
        
        string Currency { get; set; }
        
        string TradingVenue { get; set; }
        
        string Status { get; set; }
    }
}
