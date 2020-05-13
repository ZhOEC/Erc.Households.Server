using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Payments
{
    public enum PaymentStatus
    {
        New,
        Processed,
        Canceled = -1
    }
}
