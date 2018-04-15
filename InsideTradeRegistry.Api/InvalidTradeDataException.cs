using System;

namespace InsideTradeRegistry.Api
{
    public class InvalidTradeDataException : Exception
    {
        public InvalidTradeDataException()
        {
        }

        public InvalidTradeDataException(string message)
            : base(message)
        {
        }

        public InvalidTradeDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
