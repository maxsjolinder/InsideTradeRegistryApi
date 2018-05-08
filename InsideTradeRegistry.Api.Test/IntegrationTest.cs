using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace InsideTradeRegistry.Api.Test
{
    [TestCategory("Integration test")]
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public async Task VerifyOldKnownHexagonTransactionAsync()
        {
            InsideTradeRegistryApi api = new InsideTradeRegistryApi();
            var transactions = await api.GetInsideTradeTransactionsAsync(new SearchQuery
            {
                Issuer = "Hexagon AB",
                PublicationDateFrom = ToDateTime("2016-01-01 00:00:00"),
                PublicationDateTo = ToDateTime("2016-07-05 00:00:00")
            });

            Assert.AreEqual(1, transactions.Count);
            var transaction = transactions.First();
            Assert.AreEqual(ToDateTime("2016-07-04 14:04:27"), transaction.PublicationDate);
            Assert.AreEqual("Hexagon AB", transaction.Issuer);
            Assert.AreEqual("549300WJFW6ILNI4TA80", transaction.LEICode);
            Assert.AreEqual("Melker Schörling AB", transaction.Notifier);
            Assert.AreEqual("Melker Schörling", transaction.Person);
            Assert.AreEqual("Styrelseledamot/suppleant", transaction.Position);
            Assert.AreEqual("Yes", transaction.CloselyAssociated);
            Assert.AreEqual("", transaction.Amendment);
            Assert.AreEqual("", transaction.DetailsOfAmendment);
            Assert.AreEqual("Yes", transaction.InitialNotification);
            Assert.AreEqual("", transaction.PartOfShareOptionProgramme);
            Assert.AreEqual("Acquisition", transaction.NatureOfTransaction);
            Assert.AreEqual("Hexagon AB Företagscertifikat", transaction.Instrument);
            Assert.AreEqual("SE0008613608", transaction.ISIN);
            Assert.AreEqual(ToDateTime("2016-07-04 00:00:00"), transaction.TransactionDate);
            Assert.AreEqual(649833931, transaction.Volume);
            Assert.AreEqual("Amount", transaction.Unit);
            Assert.AreEqual(649833931, transaction.Price);
            Assert.AreEqual("SEK", transaction.Currency);
            Assert.AreEqual("Outside a trading venue", transaction.TradingVenue);
            Assert.AreEqual("Current", transaction.Status);
        }

        [TestMethod]
        public async Task WhenRequestingTheLast15DaysOfTransactionsThenTheyArePossibleToParseAsync()
        {
            InsideTradeRegistryApi api = new InsideTradeRegistryApi();
            var transactions = await api.GetInsideTradeTransactionsAsync(new SearchQuery
            {
                PublicationDateFrom = DateTime.Now.AddDays(-15)
            });
            Assert.AreNotEqual(0, transactions.Count);
        }

        private DateTime ToDateTime(string dateTimeString)
        {
            return DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
    }
}
