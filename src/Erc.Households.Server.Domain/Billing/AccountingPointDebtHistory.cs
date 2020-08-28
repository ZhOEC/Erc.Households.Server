using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Billing
{
    public class AccountingPointDebtHistory
    {
        public int AccountingPointId { get; set; }
        public int PeriodId { get; set; }
        public decimal DebtValue { get; set; }
    }
}
