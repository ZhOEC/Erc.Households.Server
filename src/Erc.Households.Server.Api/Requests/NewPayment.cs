using Erc.Households.Domain.Payments;
using System;

namespace Erc.Households.Api.Requests
{
    public class NewPayment
    {
        public int? BatchId { get; set; }
        public int AccountingPointId { get; set; }
        public DateTime PayDate { get; set; }
        public decimal Amount { get; set; }
        public string PayerInfo { get; set; }
        public PaymentType Type { get; set; }
    }
}