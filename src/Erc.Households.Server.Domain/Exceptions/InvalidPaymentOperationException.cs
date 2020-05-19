using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Erc.Households.Domain.Exceptions
{
    public class InvalidPaymentOperationException : Exception
    {
        public InvalidPaymentOperationException()
        {
        }

        public InvalidPaymentOperationException(string message) : base(message)
        {
        }

        public InvalidPaymentOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
