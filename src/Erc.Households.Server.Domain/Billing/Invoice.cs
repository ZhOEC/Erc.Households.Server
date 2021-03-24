using Erc.Households.CalculateStrategies.Core;
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Extensions;
using Erc.Households.Domain.Payments;
using Erc.Households.Domain.Shared;
using Erc.Households.Domain.Shared.AccountingPoints;
using Erc.Households.Domain.Shared.Billing;
using Erc.Households.Domain.Shared.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Erc.Households.Domain.Billing
{
    public class Invoice
    {
        private readonly List<InvoicePaymentItem> _invoicePaymentItems = new List<InvoicePaymentItem>();
        private readonly Action<object, string> LazyLoader;
        //private Tariff _tariff;
        private AccountingPoint _accountingPoint;
        AccountingPointExemption _exemption;
        
        protected Invoice(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public Invoice(Guid dsoCalculationId, AccountingPoint accountingPoint, DateTime fromDate, DateTime toDate, ZoneRecord zoneRecord, Usage usageT1, Usage usageT2 = null, Usage usageT3 = null)
        {
            DsoConsumptionId = dsoCalculationId;
            AccountingPoint = accountingPoint;
            AccountingPointId = accountingPoint.Id;
            Period = accountingPoint.BranchOffice.CurrentPeriod;
            FromDate = fromDate;
            ToDate = toDate;
            UsageT1 = usageT1;
            UsageT2 = usageT2;
            UsageT3 = usageT3;
            Tariff = accountingPoint.TariffsHistory.OrderByDescending(t => t.StartDate).FirstOrDefault(t => t.StartDate < toDate).Tariff;
            IncomingBalance = accountingPoint.Debt;
            Type = accountingPoint.BranchOffice.CurrentPeriod.StartDate == fromDate ? InvoiceType.Common : InvoiceType.Recalculation;
            ZoneRecord = zoneRecord;
        }

        public Invoice(int accountingPointId, int periodId, DateTime fromDate, DateTime toDate, int tariffId, Usage usageT1, Usage usageT2 = null, Usage usageT3 = null, InvoiceType type = InvoiceType.Common, string note = null, decimal? exemptionCoeff = null)
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
            ExemptionCoeff = exemptionCoeff > 1 ? exemptionCoeff / 100 : exemptionCoeff;
            AccountingPointId = accountingPointId;
        }

        public Invoice(Guid dsoId, int periodId, decimal incomingBalance, DateTime fromDate, DateTime toDate, string counterSerialNumber, Tariff tariff, 
            ZoneRecord zoneRecord, Usage usageT1, Usage usageT2, Usage usageT3, 
            AccountingPointExemption exemption = null, InvoiceType type = InvoiceType.Common, string note = null)
        {
            DsoConsumptionId = dsoId;
            IncomingBalance = incomingBalance;
            PeriodId = periodId;
            FromDate = fromDate;
            ToDate = toDate;
            UsageT1 = usageT1;
            UsageT2 = usageT2;
            UsageT3 = usageT3;
            Tariff = tariff;
            Note = note;
            Type = type;
            ExemptionCoeff = (exemption?.Category.Coeff ?? 0) > 1 ? (exemption?.Category.Coeff ?? 0) / 100 : (exemption?.Category.Coeff ?? 0);
            CounterSerialNumber = counterSerialNumber;
            _exemption = exemption;
            ZoneRecord = zoneRecord;
        }

        public int Id { get; private set; }
        public int PeriodId { get; set; }
        public int AccountingPointId { get; set; }
        public DateTime Date { get; private set; } = DateTime.Now;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Usage UsageT1 { get; set; }
        public Usage UsageT2 { get; set; }
        public Usage UsageT3 { get; set; }
        public decimal TotalUnits { get; private set; }
        public decimal? ExemptionCoeff { get; private set; }
        public int TotalDiscountUnits => UsageT1.DiscountUnits + (UsageT2?.DiscountUnits ?? 0) + (UsageT3?.DiscountUnits ?? 0);
        public decimal TotalDiscount { get; private set; }
        public decimal TotalAmountDue { get; private set; }
        public decimal TotalCharge { get; private set; }
        public decimal IncomingBalance { get; init; }
        public string CounterSerialNumber { get; private set; }
        public decimal TotalPaid => InvoicePaymentItems.Sum(i => i.Amount);
        public bool IsPaid => TotalAmountDue == TotalPaid;
        public int TariffId { get; private set; }
        public string Note { get; init; }
        public InvoiceType Type { get; init; } = InvoiceType.Common;
        public int ZoneCount => UsageT2 is null ? 1 : (UsageT3 is null ? 2 : 3);
        public Tariff Tariff { get; init; }
        public AccountingPoint AccountingPoint
        {
            get => LazyLoader.Load(this, ref _accountingPoint);
            init => _accountingPoint = value; 
        }
        public IEnumerable<InvoicePaymentItem> InvoicePaymentItems => _invoicePaymentItems.AsReadOnly();
        public Period Period { get; init; }
        public ZoneRecord ZoneRecord { get; init; }
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

        public async Task CalculateAsync(ICalculateStrategy calculateStrategy)
        {
            var exemption = AccountingPoint.Exemptions.FirstOrDefault(t => t.EffectiveDate <= FromDate && (t.EndDate ?? DateTime.MaxValue) > ToDate);

            await calculateStrategy.Calculate(new CalculationRequest
            {
                FromDate = FromDate,
                ToDate = ToDate,
                UsageT1 = UsageT1,
                UsageT2 = UsageT2,
                UsageT3 = UsageT3,
                Tariff = Tariff,
                AccountingPointId = AccountingPointId,
                ZoneRecord = ZoneRecord,
                InvoiceType = Type,
                ExemptionData = exemption != null ? new ExemptionData
                {
                    ExemptionDiscountNorms = AccountingPoint.UsageCategory.ExemptionDiscountNorms,
                    ExemptionPercent = exemption.Category.Coeff,
                    HeatingCorrection = AccountingPoint.BuildingType.HeataingCorrection,
                    IsCentralizedHotWaterSupply = AccountingPoint.IsCentralizedHotWaterSupply ?? true,
                    CanBeUsedElectricWaterHeater = AccountingPoint.CanBeUsedElectricWaterHeater,
                    IsGasWaterHeaterInstalled = AccountingPoint.IsGasWaterHeaterInstalled ?? true,
                    NumberOfPeople = exemption.PersonsNumber > 0 ? exemption.PersonsNumber : 1,
                    IsElectricHeatig = AccountingPoint.UsageCategory.Id > 2,
                    UseDiscountLimit = exemption.Category.HasLimit ?? exemption.HasLimit
                } : null
            });

            TotalUnits = UsageT1.Units + (UsageT2?.Units ?? 0) + (UsageT3?.Units ?? 0);
            TotalDiscount = UsageT1.Discount + (UsageT2?.Discount ?? 0) + (UsageT3?.Discount ?? 0);
            //TotalDiscountUnits = UsageT1.DiscountUnits + (UsageT2?.DiscountUnits ?? 0) + (UsageT3?.DiscountUnits ?? 0);
            TotalCharge = UsageT1.Charge + (UsageT2?.Charge ?? 0) + (UsageT3?.Charge ?? 0);
            TotalAmountDue = TotalCharge - TotalDiscount;
        }
    }
}
