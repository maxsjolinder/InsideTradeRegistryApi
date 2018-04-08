using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Globalization;
using System.Threading;

namespace InsideTradeRegistry.Api
{
    internal class InsideTradeRegistryHttpClient
    {
        private struct HeaderTypeMapping
        {
            public PropertyInfo PropertyInfo { get; set; }
            public string HeaderText { get; set; }
            public int ColumnIndex { get; set; }
            public IFormatProvider FormatProvider { get; set; }
        }
        private static readonly HttpClient httpClient = new HttpClient();

        internal InsideTradeRegistryHttpClient()
        {
        }

        public async Task<IList<ITradeTransaction>> GetInsideTradeTransactionsAsync(SearchQuery searchQuery)
        {
            var url = GetCsvUrl(searchQuery);

            return await GetTransationsAsync(url);
        }

        //TODO implement better csv parser supporting "" strings and """" strings.
        private async Task<IList<ITradeTransaction>> GetTransationsAsync(string url)
        {
            var byteArray = await httpClient.GetByteArrayAsync(url);
            var unicodeString = Encoding.Unicode.GetString(byteArray);
            var dataLines = unicodeString.Split(new[] { "\r\n" }, StringSplitOptions.None);

            if (dataLines.Count() < 2)
            {
                // No transactions exist
                return new List<ITradeTransaction>();
            }

            var headerTexts = dataLines[0].TrimEnd(';').Split(';');
            var mappings = BuildCsvHeaderToTypeMappings(headerTexts);

            var transactions = new List<ITradeTransaction>();
            for (int i = 1; i < dataLines.Count(); i++)
            {
                var dataLine = dataLines[i].TrimEnd(';');
                if (dataLine.Length == 0)
                    continue;

                var dataElements = dataLine.Split(';');

                var transaction = CreateTradeTransaction(dataElements, mappings);
                transactions.Add(transaction);
            }

            return transactions;
        }

        private TradeTransaction CreateTradeTransaction(string[] data, List<HeaderTypeMapping> mappings)
        {
            if (data.Length != mappings.Count)
            {
                // TODO Remove check when code is stable
                throw new ArgumentException("Arguments have different size.");
            }

            var transaction = new TradeTransaction();
            foreach (var mapping in mappings)
            {
                try
                {
                    var dataAsString = data[mapping.ColumnIndex];
                    var typedData = Convert.ChangeType(dataAsString, mapping.PropertyInfo.PropertyType, mapping.FormatProvider);
                    mapping.PropertyInfo.SetValue(transaction, typedData);
                }
                catch (InvalidCastException ex)
                {
                    // TODO Add error handling
                }
                catch (FormatException ex)
                {
                    // TODO Add error handling
                }

            }
            return transaction;
        }


        private List<HeaderTypeMapping> BuildCsvHeaderToTypeMappings(string[] headerStrings)
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
                        FormatProvider = formatProvider
                    });
                }
            }

            return typeMappings;
        }

        private string GetCsvUrl(SearchQuery searchQuery)
        {
            var issuer = "";
            var person = "";

            if (!searchQuery.PerformFullSearch)
            {
                issuer = searchQuery.Issuer.Trim().Replace(' ', '+');
                person = searchQuery.Person.Trim().Replace(' ', '+');
            }

            var customUrl = $"https://marknadssok.fi.se/publiceringsklient/en-GB/Search/Search?SearchFunctionType=Insyn&Utgivare={issuer}&PersonILedandeSt%C3%A4llningNamn={person}&Transaktionsdatum.From=&Transaktionsdatum.To=&Publiceringsdatum.From=&Publiceringsdatum.To=&button=export&Page=1";
            return customUrl;
        }
    }
}