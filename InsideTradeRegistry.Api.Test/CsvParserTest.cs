using Microsoft.VisualStudio.TestTools.UnitTesting;
using InsideTradeRegistry.Api.Parser;

namespace InsideTradeRegistry.Api.Test
{
    [TestClass]
    public class CsvParserTest
    {
        [TestMethod]
        public void GivenASimpleCsvStringWhenParsedThenWordsAreReturnedSeparated()
        {
            SimpleCsvParser parser = new SimpleCsvParser();
            var result = parser.ParseData("This;;is;a;simple;string,;\r\nused;for;testing;;;purposes.;\r\nHave;a;nice;day;today;.;");
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(6, result[0].Count);
            Assert.AreEqual(6, result[1].Count);
            Assert.AreEqual(6, result[2].Count);

            Assert.AreEqual("This", result[0][0]);
            Assert.AreEqual("", result[0][1]);
            Assert.AreEqual("is", result[0][2]);
            Assert.AreEqual("a", result[0][3]);
            Assert.AreEqual("simple", result[0][4]);
            Assert.AreEqual("string,", result[0][5]);

            Assert.AreEqual("used", result[1][0]);
            Assert.AreEqual("for", result[1][1]);
            Assert.AreEqual("testing", result[1][2]);
            Assert.AreEqual("", result[1][3]);
            Assert.AreEqual("", result[1][4]);
            Assert.AreEqual("purposes.", result[1][5]);

            Assert.AreEqual("Have", result[2][0]);
            Assert.AreEqual("a", result[2][1]);
            Assert.AreEqual("nice", result[2][2]);
            Assert.AreEqual("day", result[2][3]);
            Assert.AreEqual("today", result[2][4]);
            Assert.AreEqual(".", result[2][5]);
        }           
                    
        [TestMethod]
        public void GivenCsvStringWithSemicolonBetweenQuotationMarksWhenParsedThenStringIsSplitCorrectly()
        {
            SimpleCsvParser parser = new SimpleCsvParser();
            var str = @"""Hello World;"";said;the;computer;";
            var result = parser.ParseData(str);
            Assert.AreEqual(1, result.Count);
            var line = result[0];
            Assert.AreEqual(4, line.Count);
            Assert.AreEqual("Hello World;", line[0]);
            Assert.AreEqual("said", line[1]);
            Assert.AreEqual("the", line[2]);
            Assert.AreEqual("computer", line[3]);
        }

        [TestMethod]
        public void GivenCsvStringWithQuotationMarksBetweenQuotationMarksWhenParsedThenStringIsSplitCorrectly()
        {
            SimpleCsvParser parser = new SimpleCsvParser();
            var str = @" Hello;I'm ;""called """"fred""""""; or ;""""""freddy"""""";";

            var result = parser.ParseData(str);
            Assert.AreEqual(1, result.Count);
            var line = result[0];
            Assert.AreEqual(5, line.Count);
            Assert.AreEqual(" Hello", line[0]);
            Assert.AreEqual("I'm ", line[1]);
            Assert.AreEqual(@"called ""fred""", line[2]);
            Assert.AreEqual(" or ", line[3]);
            Assert.AreEqual(@"""freddy""", line[4]);
        }
        
        [TestMethod]
        public void GivenEmptyCsvStringWhenParsedThenEmptyListIsReturned()
        {
            SimpleCsvParser parser = new SimpleCsvParser();       
            var result = parser.ParseData("");
            Assert.AreEqual(0, result.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void GivenCsvStringWithBlanksOnlyWhenParsedThenEmptyListIsReturned()
        {
            SimpleCsvParser parser = new SimpleCsvParser();
            var result = parser.ParseData("     ");
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GivenCsvStringWithSingleSemicolonWhenParsedThenOnlyEmptyStringIsReturned()
        {
            SimpleCsvParser parser = new SimpleCsvParser();
            var result = parser.ParseData(";");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].Count);
            Assert.AreEqual("", result[0][0]);
        }

        [TestMethod]
        public void GivenCsvStringWithMultipleSemicolonsWhenParsedThenEqualAmountOfEmptyStringsIsReturned()
        {
            SimpleCsvParser parser = new SimpleCsvParser();
            var result = parser.ParseData(";;;\r\n;");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[0].Count);
            Assert.AreEqual(1, result[1].Count);
            Assert.AreEqual("", result[0][0]);
            Assert.AreEqual("", result[0][1]);
            Assert.AreEqual("", result[0][2]);
            Assert.AreEqual("", result[1][0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void GivenCsvStringWithMissingEndingSemicolonWhenParsedThenParseExceptionIsThown()
        {
            SimpleCsvParser parser = new SimpleCsvParser();
            parser.ParseData("string;missing;\r\n ending; semicolon");           
        }
    }
}
