using InsideTradeRegistry.Api.HttpClient;
using InsideTradeRegistry.Api.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InsideTradeRegistry.Api.Test
{
    [TestClass]
    public class TradeRegistryServiceTest
    {
        private InsideTradeRegistryService tradeHttpClient;
        private Mock<IInsideTradeRegistryHttpClient> httpClientMock;

        [TestInitialize]
        public void Setup()
        {
            httpClientMock = new Mock<IInsideTradeRegistryHttpClient>();
            tradeHttpClient = new InsideTradeRegistryService(httpClientMock.Object);
        }

        [TestMethod]
        public async Task GivenFullSetOfCsvDataWhenFetchingTransactionsThenDataIsMappedToTheCorrectTransactionPropertiesAsync()
        {
            // Arrange
            var pubDateString = "01/03/2018 14:07:13";
            var pubDate = ToDateTime(pubDateString);
            var issuer = "Company AB";
            var lei = "GHLA31G451234CAS";
            var notifier = "Firm ABC";
            var pdmr = "John Johnson";
            var position = "CEO";
            var closelyAssociated = "Yes";
            var amendment = "Yes";
            var amendmentDetails = "Corrected wrong price.";
            var initialNotification = "Yes";
            var shareOptionProgram = "Yes";
            var natureOfTransaction = "Acquisition";
            var instrument = "Company AB";
            var isin = "123123";
            var transactionDateString = "14/12/2017 00:00:00";
            var transactionDate = ToDateTime(transactionDateString);
            var volume = 5407;
            var unit = "Quantity";
            var price = 39.6;
            var priceString = price.ToString(CultureInfo.InvariantCulture);
            var currency = "SEK";
            var tradingVenue = "NASDAQ STOCKHOLM AB";
            var status = "Current";

            var dataHeader = "Publication date;Issuer;LEI-code;Notifier;Person discharging managerial responsibilities;Position;Closely associated;Amendment;Details of amendment;Initial notification;Linked to share option programme;Nature of transaction;Instrument;ISIN;Transaction date;Volume;Unit;Price;Currency;Trading venue;Status;\r\n";
            var dataRow = $"{pubDateString};{issuer};{lei};{notifier};{pdmr};{position};{closelyAssociated};{amendment};{amendmentDetails};{initialNotification};{shareOptionProgram};{natureOfTransaction};{instrument};{isin};{transactionDateString};{volume};{unit};{priceString};{currency};{tradingVenue};{status};";
            var data = dataHeader + dataRow;
            var byteArrayData = Encoding.Unicode.GetBytes(data);
            httpClientMock.Setup(x => x.GetByteArrayAsync(It.IsAny<string>())).Returns(Task.FromResult<byte[]>(byteArrayData));

            // Act
            var transactions = await tradeHttpClient.GetInsideTradeTransactionsAsync(new SearchQuery());

            // Assert
            Assert.AreEqual(1, transactions.Count);
            var trans = transactions.First();
            Assert.IsNotNull(trans);
            Assert.AreEqual(pubDate, trans.PublicationDate);
            Assert.AreEqual(issuer, trans.Issuer);
            Assert.AreEqual(lei, trans.LEICode);
            Assert.AreEqual(notifier, trans.Notifier);
            Assert.AreEqual(pdmr, trans.Person);
            Assert.AreEqual(position, trans.Position);
            Assert.AreEqual(closelyAssociated, trans.CloselyAssociated);
            Assert.AreEqual(amendment, trans.Amendment);
            Assert.AreEqual(amendmentDetails, trans.DetailsOfAmendment);
            Assert.AreEqual(initialNotification, trans.InitialNotification);
            Assert.AreEqual(shareOptionProgram, trans.PartOfShareOptionProgramme);
            Assert.AreEqual(natureOfTransaction, trans.NatureOfTransaction);
            Assert.AreEqual(instrument, trans.Instrument);
            Assert.AreEqual(isin, trans.ISIN);
            Assert.AreEqual(transactionDate, trans.TransactionDate);
            Assert.AreEqual(volume, trans.Volume);
            Assert.AreEqual(unit, trans.Unit);
            Assert.AreEqual(price, trans.Price);
            Assert.AreEqual(currency, trans.Currency);
            Assert.AreEqual(tradingVenue, trans.TradingVenue);
            Assert.AreEqual(status, trans.Status);
        }


        private static DateTime ToDateTime(string apiDateString)
        {
            var apiFormat = "dd/MM/yyyy H:mm:ss";
            DateTime result;
            Assert.IsTrue(DateTime.TryParseExact(apiDateString, apiFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result));
            return result;
        }

        [TestMethod]
        public async Task GivenInvalidFormattedDateTimeWhenFetchingTransactionsThenInvalidTradeDataExceptionIsThrownAsync()
        {
            // Arrange
            var badlyFormattedDateString = "01/03/201x 14:07:13";
            var dataHeader = "Publication date;\r\n";
            var dataRow = $"{badlyFormattedDateString};";
            var data = dataHeader + dataRow;
            var byteArrayData = Encoding.Unicode.GetBytes(data);
            httpClientMock.Setup(x => x.GetByteArrayAsync(It.IsAny<string>())).Returns(Task.FromResult<byte[]>(byteArrayData));

            // Act & Assert
            try
            {
                await tradeHttpClient.GetInsideTradeTransactionsAsync(new SearchQuery());
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(InvalidTradeDataException), ex.GetType());
                Assert.AreEqual(typeof(FormatException), ex.InnerException.GetType());
                return;
            }
            Assert.Fail($"No {typeof(InvalidTradeDataException)} was thrown.");
        }

        [TestMethod]
        public async Task GivenInvalidFormattedDoubleWhenFetchingTransactionsThenInvalidTradeDataExceptionIsThrownAsync()
        {
            // Arrange
            var badlyFormattedPriceString = "412.41g";
            var dataHeader = "Price;\r\n";
            var dataRow = $"{badlyFormattedPriceString};";
            var data = dataHeader + dataRow;
            var byteArrayData = Encoding.Unicode.GetBytes(data);
            httpClientMock.Setup(x => x.GetByteArrayAsync(It.IsAny<string>())).Returns(Task.FromResult<byte[]>(byteArrayData));

            // Act & Assert
            try
            {
                await tradeHttpClient.GetInsideTradeTransactionsAsync(new SearchQuery());
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(InvalidTradeDataException), ex.GetType());
                Assert.AreEqual(typeof(FormatException), ex.InnerException.GetType());
                return;
            }
            Assert.Fail($"No {typeof(InvalidTradeDataException)} was thrown.");
        }


        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public async Task GivenBadlyFormattedCsvDataWhenFetchingTransactionsThenParseExceptionIsThrownAsync()
        {
            var dataHeader = "Issuer;Price;\r\n";
            var badlyFormattedData = "Company;12.9\r\n";
            var data = dataHeader + badlyFormattedData;
            var byteArrayData = Encoding.Unicode.GetBytes(data);
            httpClientMock.Setup(x => x.GetByteArrayAsync(It.IsAny<string>())).Returns(Task.FromResult<byte[]>(byteArrayData));
            await tradeHttpClient.GetInsideTradeTransactionsAsync(new SearchQuery());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTradeDataException))]
        public async Task GivenCsvHeaderAndCsvDataOfDifferentLengthsWhenFetchingTransactionsThenInvalidTradeDataExceptionIsThrownAsync()
        {
            var threeColHeader = "Issuer;Price;extra col;\r\n";
            var twoColData = "Company;12.9;\r\n";
            var data = threeColHeader + twoColData;
            var byteArrayData = Encoding.Unicode.GetBytes(data);
            httpClientMock.Setup(x => x.GetByteArrayAsync(It.IsAny<string>())).Returns(Task.FromResult<byte[]>(byteArrayData));
            await tradeHttpClient.GetInsideTradeTransactionsAsync(new SearchQuery());
        }

        [TestMethod]
        public async Task GivenCsvHeaderLabelsThatDoNotMatchPredefinedTypeMappingWhenFetchingTransactionsThenExistingTypeMappingsAreReturnedAsync()
        {
            var threeColHeader = "Random Col 1;Issuer;Random Col 2;Price;Random col 3;\r\n";
            var twoColData = "Random data 1;Company;Random data 2;12.9;Random data 3;\r\n";
            var data = threeColHeader + twoColData;
            var byteArrayData = Encoding.Unicode.GetBytes(data);
            httpClientMock.Setup(x => x.GetByteArrayAsync(It.IsAny<string>())).Returns(Task.FromResult<byte[]>(byteArrayData));
            var transactions = await tradeHttpClient.GetInsideTradeTransactionsAsync(new SearchQuery());
            Assert.AreEqual(1, transactions.Count);
            var transaction = transactions.First();
            Assert.AreEqual("Company", transaction.Issuer);
            Assert.AreEqual(12.9, transaction.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task GivenConnectionProblemWithExternalApiWhenFetchingTransactionsThenHttpRequestExceptionIsTrownAsync()
        {
            httpClientMock.Setup(x => x.GetByteArrayAsync(It.IsAny<string>())).Throws(new HttpRequestException("Unable to connect"));
            var transactions = await tradeHttpClient.GetInsideTradeTransactionsAsync(new SearchQuery());
        }

        [TestMethod]
        public async Task TestBooleanResponsesSeparatelyAsync()
        {
            // Tests boolean separately since they cannot otherwise be distinguished from each other
            await SetupAndAssertBooleanResponseAsync(closelyAssociated: "Yes", amendment: "No", initialNotification: "No", shareOptionProgram:"No");
            await SetupAndAssertBooleanResponseAsync(closelyAssociated: "No", amendment: "Yes", initialNotification: "No", shareOptionProgram: "No");
            await SetupAndAssertBooleanResponseAsync(closelyAssociated: "No", amendment: "No", initialNotification: "Yes", shareOptionProgram: "No");
            await SetupAndAssertBooleanResponseAsync(closelyAssociated: "No", amendment: "No", initialNotification: "No", shareOptionProgram: "Yes");  
        }

        private async Task SetupAndAssertBooleanResponseAsync(string closelyAssociated, string amendment, string initialNotification, string shareOptionProgram)
        {
            // Arrange
            var dataHeader = "Closely associated;Amendment;Initial notification;Linked to share option programme;\r\n";
            var dataRow = $"{closelyAssociated};{amendment};{initialNotification};{shareOptionProgram};";
            var data = dataHeader + dataRow;
            var byteArrayData = Encoding.Unicode.GetBytes(data);
            httpClientMock.Setup(x => x.GetByteArrayAsync(It.IsAny<string>())).Returns(Task.FromResult<byte[]>(byteArrayData));

            // Act
            var transactions = await tradeHttpClient.GetInsideTradeTransactionsAsync(new SearchQuery());

            // Assert
            var transaction = transactions.Single();
            Assert.AreEqual(closelyAssociated, transaction.CloselyAssociated);
            Assert.AreEqual(amendment, transaction.Amendment);
            Assert.AreEqual(initialNotification, transaction.InitialNotification);
            Assert.AreEqual(shareOptionProgram, transaction.PartOfShareOptionProgramme);
        }        
    }
}
