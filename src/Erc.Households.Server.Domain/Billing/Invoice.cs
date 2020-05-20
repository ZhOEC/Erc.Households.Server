using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Payments;
using Erc.Households.Domain.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erc.Households.Domain.Billing
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
        public int? PreviousT2MeterReading { get; set; }
        public int? PresentT2MeterReading { get; set; }
        public int? PreviousT3MeterReading { get; set; }
        public int? PresentT3MeterReading { get; set; }
        public int T1Usage { get; set; }
        public int? T2Usage { get; set; }
        public int? T3Usage { get; set; }
        public decimal T1Sales { get; set; }
        public decimal? T2Sales { get; set; }
        public decimal? T3Sales { get; set; }
        public decimal TotalSales { get; set; }
        public decimal IncomingBalance { get; set; }
        public string CounterSerialNumber { get; set; }
        public decimal TotalPaid => InvoicePaymentItems.Sum(i => i.Amount);
        public bool IsPaid => TotalSales == TotalPaid;
        public int TariffId { get; set; }
        public string Note { get; set; }
        public Tariff Tariff { get; set; }
        public IEnumerable<InvoiceDetail> InvoiceDetails { get; set; }
        public IEnumerable<InvoicePaymentItem> InvoicePaymentItems => _invoicePaymentItems.AsReadOnly();
        public Period Period { get; private set; }
        public ZoneRecord ZoneRecord { get; set; }
        public Guid DsoConsumptionId { get; set; }
        public InvoicePaymentItem Pay(Payment payment)
        {
            if (payment.Amount < 0) throw new InvalidOperationException($"Платіж {payment.Id} не може бути оброблений. Сумма платежу має бути додатньою.");
            decimal paidAmount = payment.Balance;
            if ((TotalSales - TotalPaid) < payment.Balance) paidAmount = TotalSales - TotalPaid;
            var ipi = new InvoicePaymentItem(this, payment, paidAmount);
            _invoicePaymentItems.Add(ipi);
            return ipi;
        }
        public InvoicePaymentItem TakePaymentBack(Payment payment)
        {
            if (payment.Amount >= 0) throw new InvalidOperationException($"Платіж {payment.Id} не може бути оброблений. Сумма платежу має бути відємною.");
            decimal paidAmount = payment.Balance;
            if (TotalPaid < (1 - payment.Balance)) paidAmount = 1 - TotalPaid;
            var ipi = new InvoicePaymentItem(this, payment, paidAmount);
            _invoicePaymentItems.Add(ipi);
            return ipi;
        }
    }
}
