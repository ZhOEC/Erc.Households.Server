using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Extensions;
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
        private readonly List<InvoicePaymentItem> _invoicePaymentItems = new List<InvoicePaymentItem>();
        private readonly Action<object, string> LazyLoader;
        private Tariff _tariff;

        protected Invoice(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public Invoice(int periodId, DateTime fromDate, DateTime toDate, int tariffId, Usage usageT1, InvoiceType type = InvoiceType.Common, int? accountingPointExemptionId = null, string note = null)
            : this(periodId, fromDate, toDate, tariffId, usageT1, null, null, type, accountingPointExemptionId, note)
        {
        }

        public Invoice(int periodId, DateTime fromDate, DateTime toDate, int tariffId, Usage usageT1, Usage usageT2, InvoiceType type = InvoiceType.Common, int? accountingPointExemptionId = null, string note = null)
            : this(periodId, fromDate, toDate, tariffId, usageT1, usageT2, null, type, accountingPointExemptionId, note)
        {

        }

        public Invoice(int periodId, DateTime fromDate, DateTime toDate, int tariffId, Usage usageT1, Usage usageT2, Usage usageT3, InvoiceType type = InvoiceType.Common, int? accountingPointExemptionId = null, string note = null)
        {
            PeriodId = periodId;
            FromDate = fromDate;
            ToDate = toDate;
            UsageT1 = usageT1;
            UsageT2 = usageT2;
            UsageT3 = usageT3;
            TariffId = tariffId;
            Note = note;
            Type = type;
            //AccountingPointExemptionId = accountingPointExemptionId;
        }

        public int Id { get; private set; }
        public int PeriodId { get; private set; }
        public int AccountingPointId { get; private set; }
        public DateTime Date { get; private set; } = DateTime.Now;
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public Usage UsageT1 { get; private set; }
        public Usage UsageT2 { get; private set; }
        public Usage UsageT3 { get; private set; }
        public int TotalUnits { get; private set; }
        public int TotalDiscountUnits => UsageT1.DiscountUnits + (UsageT2?.DiscountUnits ?? 0) + (UsageT3?.DiscountUnits ?? 0);
        public decimal TotalDiscount { get; private set; }
        public decimal TotalAmountDue { get; private set; }
        public decimal TotalCharge { get; private set; }
        public decimal IncomingBalance { get; private set; }
        public string CounterSerialNumber { get; private set; }
        public decimal TotalPaid => InvoicePaymentItems.Sum(i => i.Amount);
        public bool IsPaid => TotalAmountDue == TotalPaid;
        public int TariffId { get; private set; }
        public string Note { get; private set; }
        public InvoiceType Type { get; private set; }
        public int ZoneCount => UsageT2 is null ? 1 : (UsageT3 is null ? 2 : 3);
        public Tariff Tariff 
        {
            get => LazyLoader.Load(this, ref _tariff);
            private set { _tariff = value; }
        }
        public IEnumerable<InvoicePaymentItem> InvoicePaymentItems => _invoicePaymentItems.AsReadOnly();
        public Period Period { get; private set; }
        public ZoneRecord ZoneRecord { get; set; }
        public Guid DsoConsumptionId { get; set; }

        public InvoicePaymentItem Pay(Payment payment)
        {
            if (payment.Amount < 0) throw new InvalidOperationException($"Платіж {payment.Id} не може бути оброблений. Сумма платежу має бути додатньою.");
            decimal paidAmount = payment.Balance;
            if ((TotalAmountDue - TotalPaid) < payment.Balance) paidAmount = TotalAmountDue - TotalPaid;
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
        public void Calculate()
        {
            if (Type == InvoiceType.Recalculation)
            {
                var tariffRate = Tariff.Rates.OrderByDescending(t => t.StartDate).First(t => t.StartDate <= FromDate);
              // if (AccountingPoint.)
            }
        }
    }
}
