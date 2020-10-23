using System;
using Erc.Households.Domain.Billing;

namespace Erc.Households.Domain.Payments
{
    public class KFKPayment
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public decimal Sum { get; set; }
        public DateTime EnterDate { get; set; }
        public string OperatorName { get; set; }
        public Period Period { get; set; }
    }
}
