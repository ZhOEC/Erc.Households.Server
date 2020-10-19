using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Exceptions
{
    public class InvalidAccountingPointDataException: Exception
    {
        public InvalidAccountingPointDataException()
        {
        }

        public InvalidAccountingPointDataException(string message) : base(message)
        {
        }

        public InvalidAccountingPointDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
