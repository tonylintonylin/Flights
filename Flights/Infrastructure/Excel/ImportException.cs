using System;

namespace Flights
{
    public class ImportException : Exception
    {
        public ImportException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
