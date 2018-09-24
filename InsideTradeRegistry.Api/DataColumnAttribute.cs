using System;

namespace InsideTradeRegistry.Api
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal class DataColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public string CultureToUse { get; set; } = null;
               
        internal virtual object ConvertStringToType(string stringToConvert, Type targetType, IFormatProvider formatProvider)
        {
            return Convert.ChangeType(stringToConvert, targetType, formatProvider);
        }
    }
}
