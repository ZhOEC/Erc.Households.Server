using System;
using MediatR;

namespace Erc.Households.Commands.Payments
{
    public class CreatePaymentCommand : IRequest
    {
        public CreatePaymentCommand(int accountingPointId, DateTime payDate, decimal amount, string payerInfo, int type, int? batchId = null)
        {
            AccountingPointId = accountingPointId;
            PayDate = payDate;
            Amount = amount;
            PayerInfo = payerInfo;
            Type = type;
            BatchId = batchId;
        }

        public int AccountingPointId { get; set; }
        public DateTime PayDate { get; set; }
        public decimal Amount { get; set; }
        public string PayerInfo { get; set; }
        public int Type { get; set; }
        public int? BatchId { get; set; }
    }
}
