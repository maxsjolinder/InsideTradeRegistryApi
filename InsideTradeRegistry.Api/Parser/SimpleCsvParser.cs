using System;
using System.Collections.Generic;

namespace InsideTradeRegistry.Api.Parser
{
    internal class SimpleCsvParser : IParser
    {
        private string lineToParse;
        
        public IList<IList<string>> ParseData(string data)
        {
            var lines = SplitLines(data);
            var result = new List<IList<string>>();
            foreach (var line in lines)
            {
                var parsedElements = ParseLine(line);
                if (parsedElements.Count > 0)
                {
                    result.Add(parsedElements);
                }
            }

            return result;
        }

        private string[] SplitLines(string data)
        {
            return data.Split(new[] { "\r\n" }, StringSplitOptions.None);
        }

        private List<string> ParseLine(string line)
        {
            lineToParse = line;
            var result = new List<string>();
            while(HasMoreElementsToParse())
            {
                result.Add(GetNextParsedElement());
            }

            return result;
        }

        private bool HasMoreElementsToParse()
        {
            return lineToParse != "";
        }

        private string GetNextParsedElement()
        {
            if(lineToParse.Length < 1)
            {
                throw new ParseException("Nothing to parse.");
            }
                        
            bool surroundedByQuotationMarks = false;
            var startIndex = 0;
            int endIndex;
            int endDelimiterLength = 1;
            var startChar = lineToParse[0];
            if (startChar == '\"')
            {
                startIndex = 1;
                endIndex = lineToParse.IndexOf("\";", 1);                
                surroundedByQuotationMarks = true;
                endDelimiterLength = 2;
            }
            else
            {
                endIndex = lineToParse.IndexOf(";");
            }

            if (endIndex < 0)
            {
                throw new ParseException($"No ending delimiter was found on string {lineToParse}");
            }

            var elementLength = (endIndex - startIndex);
            var element = lineToParse.Substring(startIndex, elementLength);
            lineToParse = lineToParse.Substring(endIndex + endDelimiterLength);

            if (surroundedByQuotationMarks)
            {
                // Double quotes should be replaced by a single quote.
                element = element.Replace("\"\"", "\"");
            }        
                        
            return element;            
        }
    }
}
