using Erc.Households.Domain.Payments;
using System;

namespace Erc.Households.Api.Responses
{
    public class Payment
    {
        public int Id { get; private set; }
        public int PeriodId { get; private set; }
        public int BatchId { get; private set; }
        public int? AccountingPointId { get; private set; }
        public DateTime PayDate { get; set; }
        public DateTime EnterDate { get; set; }
        public decimal Amount { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string PayerInfo { get; private set; }
        public string AccountingPointName { get; private set; }
        public PaymentType Type { get; private set; }
    }
}
