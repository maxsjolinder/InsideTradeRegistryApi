using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
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
            Assert.AreEqual("Hexagon AB Företagscertifikat", transaction.InstrumentName);
            Assert.AreEqual("", transaction.InstrumentType);
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

        [TestMethod]
        public async Task InsideTradeRegistryApiShouldRetrieveAllAvailableDataAsync()
        {
            var url = "https://marknadssok.fi.se/publiceringsklient/en-GB/Search/Search?SearchFunctionType=Insyn&Utgivare=Essity+ab&PersonILedandeSt%C3%A4llningNamn=&Transaktionsdatum.From=&Transaktionsdatum.To=&Publiceringsdatum.From=21%2F02%2F2017&Publiceringsdatum.To=15%2F06%2F2017&button=export&Page=1";
            var httpClient = new System.Net.Http.HttpClient();
            var byteArray = await httpClient.GetByteArrayAsync(url);
            var unicodeString = Encoding.Unicode.GetString(byteArray);

            // Contains an extra blank row
            var rows = unicodeString.Split("\r\n");
            Assert.AreEqual(7, rows.Count());

            // Contains an extra blank column
            var headerColumns = rows[0].Split(";");

            var transactionInterfaceProperties = typeof(ITradeTransaction).GetProperties();
            Assert.AreEqual(headerColumns.Count() - 1, transactionInterfaceProperties.Count(), "InsideTradeRegistryApi does not retrieve all available data. Most likely Finansinspektionen has updated their api.");
        }

        private DateTime ToDateTime(string dateTimeString)
        {
            return DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
    }
}
