using System;

namespace FusionDms.Exceptions
{
    public class InvalidConnectionStringException : Exception
    {
        public InvalidConnectionStringException(string message)
            : base(message)
        {
        }
    }
}