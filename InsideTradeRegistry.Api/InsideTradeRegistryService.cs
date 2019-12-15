using System;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Globalization;
using System.Threading;
using InsideTradeRegistry.Api.Parser;
using InsideTradeRegistry.Api.HttpClient;

namespace InsideTradeRegistry.Api
{
    internal class InsideTradeRegistryService
    {
        private struct HeaderTypeMapping
        {
            public PropertyInfo PropertyInfo { get; set; }
            public string HeaderText { get; set; }
            public int ColumnIndex { get; set; }
            public IFormatProvider FormatProvider { get; set; }

            public Func<string, Type, IFormatProvider, object> ConvertToType { get; set; }
        }
        private static IInsideTradeRegistryHttpClient httpClient;
        private readonly IParser csvParser;

        internal InsideTradeRegistryService(IInsideTradeRegistryHttpClient client)
        {
            httpClient = client;
            csvParser = new SimpleCsvParser();
        }

        public async Task<IList<ITradeTransaction>> GetInsideTradeTransactionsAsync(SearchQuery searchQuery)
        {
            var url = GetCsvUrl(searchQuery);

            return await GetTransationsAsync(url);
        }

        private async Task<IList<ITradeTransaction>> GetTransationsAsync(string url)
        {
            var byteArray = await httpClient.GetByteArrayAsync(url);
            var unicodeString = Encoding.Unicode.GetString(byteArray);
            var parsedData = csvParser.ParseData(unicodeString);

            if (parsedData.Count() < 2)
            {
                // No transactions exist
                return new List<ITradeTransaction>();
            }

            var headerElements = parsedData[0];
            var mappings = BuildCsvHeaderToTypeMappings(headerElements);

            var transactions = new List<ITradeTransaction>();
            for (int i = 1; i < parsedData.Count(); i++)
            {
                var dataLine = parsedData[i];
                if (headerElements.Count != dataLine.Count)
                {
                    throw new InvalidTradeDataException("Unable to parse Finansinspektionen trade data. Header and data have different sizes.");
                }

                var transaction = CreateTradeTransaction(dataLine, mappings);
                transactions.Add(transaction);
            }

            return transactions;
        }

        private TradeTransaction CreateTradeTransaction(IList<string> data, List<HeaderTypeMapping> mappings)
        {
            var transaction = new TradeTransaction();
            foreach (var mapping in mappings)
            {
                var dataAsString = data[mapping.ColumnIndex];
                try
                {
                    var typedData = mapping.ConvertToType(dataAsString, mapping.PropertyInfo.PropertyType, mapping.FormatProvider);
                    mapping.PropertyInfo.SetValue(transaction, typedData);
                }
                catch (Exception ex)
                {
                    if (ex is InvalidCastException || ex is FormatException)
                    {
                        throw new InvalidTradeDataException($"Unable to convert string value \"{dataAsString}\" into {mapping.PropertyInfo.PropertyType} type on property \"{mapping.PropertyInfo.Name}\".", ex);
                    }

                    throw;
                }
            }
            return transaction;
        }


        private List<HeaderTypeMapping> BuildCsvHeaderToTypeMappings(IList<string> headerStrings)
        {
            var transactionType = typeof(TradeTransaction);
            var allClassProperties = transactionType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var typeMappings = new List<HeaderTypeMapping>();

            for (int colIndex = 0; colIndex < headerStrings.Count(); colIndex++)
            {
                var header = headerStrings[colIndex];
                var targetDataProperties = allClassProperties.Where(x => x.GetCustomAttribute<DataColumnAttribute>().Name.ToLower() == header.ToLower()).ToList();

                foreach (var propertyInfo in targetDataProperties)
                {
                    IFormatProvider formatProvider = Thread.CurrentThread.CurrentCulture;
                    if (propertyInfo.GetCustomAttribute<DataColumnAttribute>().CultureToUse != null)
                    {
                        // Use custom culture if it exist.
                        formatProvider = new CultureInfo(propertyInfo.GetCustomAttribute<DataColumnAttribute>().CultureToUse);
                    }

                    typeMappings.Add(new HeaderTypeMapping
                    {
                        HeaderText = header,
                        PropertyInfo = propertyInfo,
                        ColumnIndex = colIndex,
                        FormatProvider = formatProvider,
                        ConvertToType = propertyInfo.GetCustomAttribute<DataColumnAttribute>().ConvertStringToType
                    });
                }
            }

            return typeMappings;
        }

        private string GetCsvUrl(SearchQuery searchQuery)
        {
            var issuer = searchQuery.Issuer?.Trim().Replace(' ', '+') ?? "";
            var person = searchQuery.PDMRPerson?.Trim().Replace(' ', '+') ?? "";
            var transactionDateFromStr = ToUrlString(searchQuery.TransactionDateFrom);
            var transactionDateToStr = ToUrlString(searchQuery.TransactionDateTo);
            var publicationDateFromStr = ToUrlString(searchQuery.PublicationDateFrom);
            var publicationDateToStr = ToUrlString(searchQuery.PublicationDateTo);

            var customUrl = $"https://marknadssok.fi.se/publiceringsklient/en-GB/Search/Search?SearchFunctionType=Insyn&Utgivare={issuer}&PersonILedandeSt%C3%A4llningNamn={person}&Transaktionsdatum.From={transactionDateFromStr}&Transaktionsdatum.To={transactionDateToStr}&Publiceringsdatum.From={publicationDateFromStr}&Publiceringsdatum.To={publicationDateToStr}&button=export&Page=1";
            return customUrl;
        }

        private string ToUrlString(DateTime date)
        {
            if (date == default)
            {
                return "";
            }
            else
            {
                var formatString = "dd'%2F'MM'%2F'yyyy";
                return date.ToString(formatString);
            }
        }
    }
}