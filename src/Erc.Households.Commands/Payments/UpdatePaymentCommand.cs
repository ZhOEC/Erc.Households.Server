using MediatR;
using System;

namespace Erc.Households.Api.Queries.Payments
{
    public class UpdatePaymentCommand : IRequest<Unit>
    {
        public UpdatePaymentCommand(int id, int batchId, int accountingPointId, DateTime payDate, decimal amount, string payerInfo, int type)
        {
            Id = id;
            BatchId = batchId;
            AccountingPointId = accountingPointId;
            PayDate = payDate;
            Amount = amount;
            PayerInfo = payerInfo;
            Type = type;
        }

        public int Id { get; private set; }
        public int BatchId { get; private set; }
        public int AccountingPointId { get; private set; }
        public DateTime PayDate { get; private set; }
        public decimal Amount { get; private set; }
        public string PayerInfo { get; private set; }
        public int Type { get; private set; }
    }
}
