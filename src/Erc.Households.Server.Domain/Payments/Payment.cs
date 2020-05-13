using Erc.Households.Server.Domain.AccountingPoints;
using Erc.Households.Server.Domain.Billing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Payments
{
    public class Payment
    {
        //AccountingPoint _accountingPoint;
        List<InvoicePaymentItem> _invoicePaymentItems = new List<InvoicePaymentItem>();

        public Payment(DateTime payDate, decimal amount, int periodId, string payerInfo, int? accountingPointId)
        {
            PayDate = payDate;
            Amount = amount;
            PeriodId = periodId;
            AccountingPointId = accountingPointId;
            Status = PaymentStatus.New;
            EnterDate = DateTime.Now;
            PayerInfo = payerInfo;
        }

        protected Payment() { }
        public int Id { get; private set; }
        public int PeriodId { get; private set; }
        public int BatchId { get; private set; }
        public int? AccountingPointId { get; private set; }
        public DateTime PayDate { get; set; }
        public DateTime EnterDate { get; set; }
        public decimal Amount { get; private set; }
        public AccountingPoint AccountingPoint { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string PayerInfo { get; private set; }
        public string AccountingPointName { get; private set; }
        public Period Period { get; private set; }
        public IEnumerable<InvoicePaymentItem> InvoicePaymentItems => _invoicePaymentItems.AsReadOnly();

        /// <summary>
        /// Will be remoted
        /// </summary>
        //public string AccPointName { get; set; }

        public void AddInvoicePaymentItem(InvoicePaymentItem invoicePaymentItem) => _invoicePaymentItems.Add(invoicePaymentItem);

        public void Process()
        {
            AccountingPoint.ProcessPayment(this);
            Status = PaymentStatus.Processed;
        }
    }
}
