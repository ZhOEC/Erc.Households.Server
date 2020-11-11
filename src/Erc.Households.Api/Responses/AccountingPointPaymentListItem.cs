using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.Responses
{
    public class AccountingPointPaymentListItem
    {
        public decimal Amount { get; private set; }
        public DateTime PayDate { get; private set; }
        public DateTime EnterDate { get; private set; }
        public string PeriodName { get; private set; }
        public string Source { get; private set; }
}
}
