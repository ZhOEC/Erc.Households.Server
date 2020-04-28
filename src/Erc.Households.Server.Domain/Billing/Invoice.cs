using Erc.Households.Server.Domain.AccountingPoints;
using Erc.Households.Server.Domain.Payments;
using Erc.Households.Server.Domain.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erc.Households.Server.Domain.Billing
{
    public class Invoice
    {
        List<InvoicePaymentItem> _invoicePaymentItems = new List<InvoicePaymentItem>();

        public int Id { get; set; }
        public int PeriodId { get; set; }
        public int AccountingPointId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int PreviousT1MeterReading { get; set; }
        public int PresentT1MeterReading { get; set; }
        public int PreviousT2MeterReading { get; set; }
        public int PresentT2MeterReading { get; set; }
        public int PreviousT3MeterReading { get; set; }
        public int PresentT3MeterReading { get; set; }
        public int T1Usage { get; set; }
        public int T2Usage { get; set; }
        public int T3Usage { get; set; }
        public decimal T1Sales { get; set; }
        public decimal T2Sales { get; set; }
        public decimal T3Sales { get; set; }
        public decimal TotalAmountSales { get; set; }
        public decimal AmountPaid => InvoicePaymentItems.Sum(i => i.Amount);
        public bool IsPaid => TotalAmountSales == AmountPaid;
        public int TariffId { get; set; }
        public string Note { get; set; }
        public Tariff Tariff { get; set; }
        public IEnumerable<InvoiceDetail> InvoiceDetails { get; set; }
        public IEnumerable<InvoicePaymentItem> InvoicePaymentItems => _invoicePaymentItems.AsReadOnly();
        public Period Period { get; private set; }
        public ZoneRecord ZoneRecord { get; set; }
        public Guid DsoConsumptionId { get; set; }
        public void Pay(Payment payment)
        {
            if (IsPaid)
                return;
            if (payment.Amount > 0)
            {
                decimal paymentAmount = payment.Amount;
                if ((TotalAmountSales - AmountPaid) < payment.Amount) paymentAmount = TotalAmountSales - AmountPaid;
                _invoicePaymentItems.Add(new InvoicePaymentItem(this, payment, paymentAmount));
            }
        }
    }
}
