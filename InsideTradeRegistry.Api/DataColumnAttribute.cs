using System;

namespace InsideTradeRegistry.Api
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal sealed class DataColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public string CultureToUse { get; set; } = null;
    }
}
