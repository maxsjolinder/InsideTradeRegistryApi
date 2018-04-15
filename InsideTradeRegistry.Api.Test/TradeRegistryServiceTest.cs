using InsideTradeRegistry.Api.HttpClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Globalization;
using System.Linq;
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
                var transactions = await tradeHttpClient.GetInsideTradeTransactionsAsync(new SearchQuery());
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
                var transactions = await tradeHttpClient.GetInsideTradeTransactionsAsync(new SearchQuery());
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(InvalidTradeDataException), ex.GetType());
                Assert.AreEqual(typeof(FormatException), ex.InnerException.GetType());
                return;
            }
            Assert.Fail($"No {typeof(InvalidTradeDataException)} was thrown.");
        }


        /*
         TODO Add tests:
          - Different length of data.
          - Test bools separately. Since they cannot be distinguished.
          - Wrong csv format..
          - Connection problems.
         */
    }
}
