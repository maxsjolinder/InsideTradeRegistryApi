using System;
using System.Linq;

namespace InsideTradeRegistry.Api
{
    internal class DataColumnBooleanAttribute: DataColumnAttribute
    {
        public string[] TrueStrings { get; set; } = null;
        public string[] FalseStrings { get; set; } = null;

        internal override object ConvertStringToType(string stringToConvert, Type targetType, IFormatProvider formatProvider)
        {
            if(targetType != typeof(bool))
            {
                throw new InvalidCastException($"{nameof(DataColumnBooleanAttribute)} can only be used on attributes of type {typeof(bool)}.");
            }

            if(TrueStrings == null && FalseStrings == null)
            {
                // No preconfigured strings exists, use regular conversion 
                return Convert.ChangeType(stringToConvert, targetType, formatProvider);
            }

            var foundTrueStringMatch = TrueStrings?.Any(x => x.ToLower() == stringToConvert.ToLower());
            var foundFalseStringMatch = FalseStrings?.Any(x => x.ToLower() == stringToConvert.ToLower());

            if(foundTrueStringMatch.HasValue && foundFalseStringMatch.HasValue)
            {
                if (foundTrueStringMatch.Value && foundFalseStringMatch.Value)
                {
                    throw new InvalidCastException($"Ambiguous configuration: Both true and false string contains the value {stringToConvert}.");
                }
                else if (!foundTrueStringMatch.Value && !foundFalseStringMatch.Value)
                {
                    throw new InvalidCastException($"No matching conversion was found for value {stringToConvert}.");
                }
            }

            if(foundTrueStringMatch.HasValue)
            {
                return foundTrueStringMatch;
            }
            else
            {
                return !foundFalseStringMatch;
            }
        }
    }
}
