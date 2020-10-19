using Erc.Households.CalculateStrategies.Core;
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Extensions;
using Erc.Households.Domain.Payments;
using Erc.Households.Domain.Shared;
using Erc.Households.Domain.Shared.Tariffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        //public Invoice(int accountingPointId, int periodId, DateTime fromDate, DateTime toDate, int tariffId, Usage usageT1, InvoiceType type = InvoiceType.Common,  string note = null, decimal? exemptionCoeff = null)
        //    : this(accountingPointId, periodId, fromDate, toDate, tariffId, usageT1, null, null, type, note, exemptionCoeff)
        //{
        //}

        //public Invoice(int accountingPointId, int periodId, DateTime fromDate, DateTime toDate, int tariffId, Usage usageT1, Usage usageT2, InvoiceType type = InvoiceType.Common,  string note = null, decimal? exemptionCoeff = null)
        //    : this(accountingPointId, periodId, fromDate, toDate, tariffId, usageT1, usageT2, null, type, note, exemptionCoeff)
        //{

        //}


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
        public int PeriodId { get; private set; }
        public int AccountingPointId { get; private set; }
        public DateTime Date { get; private set; } = DateTime.Now;
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public Usage UsageT1 { get; set; }
        public Usage UsageT2 { get; set; }
        public Usage UsageT3 { get; set; }
        public Usage GetUsage(int i) => i switch
        {
            1 => UsageT1,
            2 => UsageT2,
            3 => UsageT3,
            _ => throw new ArgumentOutOfRangeException()
        };
        public Usage GetUsage(Expression<Func<Usage>> expression) => expression.Compile().Invoke();
        
        public IEnumerable<Expression<Func<Usage>>> GetUsagesExpressions()
        {
            yield return () => UsageT1;
            yield return () => UsageT2;
            yield return () => UsageT3;
        }

        public decimal TotalUnits { get; private set; }
        public decimal? ExemptionCoeff { get; private set; }
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
        public Tariff Tariff { get; private set; }
        public AccountingPoint AccountingPoint
        {
            get => LazyLoader.Load(this, ref _accountingPoint);
            private set { _accountingPoint = value; }
        }
        public IEnumerable<InvoicePaymentItem> InvoicePaymentItems => _invoicePaymentItems.AsReadOnly();
        public Period Period { get; private set; }
        public ZoneRecord ZoneRecord { get; private set; }
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

        public void Calculate(ICalculateStrategy calculateStrategy)
        {
            calculateStrategy.Calculate(new CalculationRequest(FromDate, ToDate, UsageT1, UsageT2, UsageT3, Tariff));
            TotalUnits = UsageT1.Units + (UsageT2?.Units ?? 0) + (UsageT3?.Units ?? 0);
            TotalDiscount = UsageT1.Discount + (UsageT2?.Discount ?? 0) + (UsageT3?.Discount ?? 0);
            TotalCharge = UsageT1.Charge + (UsageT2?.Charge ?? 0) + (UsageT3?.Charge ?? 0);
            TotalAmountDue = TotalCharge - TotalDiscount;
        }

        public void Calculate_old(IEnumerable<Invoice> invalidInvoices=default)
        {
            var tariffRates = Tariff.Rates.Where(t => t.StartDate < ToDate).GroupBy(tr => tr.StartDate).OrderByDescending(tr => tr.Key).First();

            if (UsageT1.Units != 0 || UsageT2 != null || UsageT3 != null)
            {
                if (invalidInvoices.Any())
                {
                    UsageT1.Units += invalidInvoices.Sum(i => i.UsageT1.Units);
                    if (UsageT2 != null) UsageT2.Units += invalidInvoices.Sum(i => i.UsageT2?.Units ?? 0);
                    if (UsageT3 != null) UsageT3.Units += invalidInvoices.Sum(i => i.UsageT3?.Units ?? 0);
                }

                foreach (var tr in tariffRates.OrderBy(tr => tr.ConsumptionLimit ?? int.MaxValue))
                {
                    if (UsageT1.Units > UsageT1.Calculations.Sum(c => c.Units) || UsageT1.Units < 0) CalculateInternal(() => UsageT1, tr);
                    if (UsageT2?.Units > UsageT2?.Calculations.Sum(c => c.Units) || UsageT2?.Units < 0) CalculateInternal(() => UsageT2, tr);
                    if (UsageT3?.Units > UsageT3?.Calculations.Sum(c => c.Units) || UsageT3?.Units < 0) CalculateInternal(() => UsageT3, tr);
                }

                if (invalidInvoices.Any())
                    foreach (var usageExpression in GetUsagesExpressions())
                        AddInvalidCalculationInternal(usageExpression);
            }

            TotalUnits = UsageT1.Units + (UsageT2?.Units ?? 0) + (UsageT3?.Units ?? 0);
            TotalDiscount = UsageT1.Discount + (UsageT2?.Discount ?? 0) + (UsageT3?.Discount ?? 0);
            TotalCharge = UsageT1.Charge + (UsageT2?.Charge ?? 0) + (UsageT3?.Charge ?? 0);
            TotalAmountDue = TotalCharge - TotalDiscount;
            
            void AddInvalidCalculationInternal(Expression<Func<Usage>> usageExpr)
            {
                var usage = usageExpr.Compile().Invoke();
                if (usage is null)
                    return;

                var zoneNumber = Convert.ToInt32(((MemberExpression)usageExpr.Body).Member.Name.Substring(6));
                var calculations = invalidInvoices.SelectMany(i => i.GetUsage(usageExpr).Calculations.Where(c => c.Units != 0)).ToList();
                calculations.ForEach(c =>
                    {
                        usage.AddCalculation(new UsageCalculation
                        {
                            Units = -c.Units,
                            Charge = -c.Charge,
                            Discount = -c.Discount,
                            DiscountUnits = -c.DiscountUnits,
                            PriceValue = c.PriceValue
                        });
                    });
                
                    //var invoices = invalidInvoices.Where(ii => ii.GetUsage(zoneNumber).Units > 0);
                    //var minTariffRate = tariffRates.OrderBy(tr => tr.ConsumptionLimit ?? int.MaxValue).First();
                    //var consumptionMonthLimit = minTariffRate.ConsumptionLimit ?? int.MaxValue;

                    //if (invoices.Sum(ii => ii.TotalUnits) > consumptionMonthLimit)
                    //{
                    //    var units = zoneNumber switch
                    //    {
                    //        2 when ZoneRecord == ZoneRecord.Two => consumptionMonthLimit - UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).FirstOrDefault().Units,
                    //        3 => consumptionMonthLimit - UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).FirstOrDefault().Units + UsageT2.Calculations.Where(c => c.PriceValue == tr.Value).FirstOrDefault().Units,
                    //        _ => (int)decimal.Round(consumptionMonthLimit * usage.Units / (decimal)(UsageT1.Units + (UsageT2?.Units ?? 0) + (UsageT3?.Units ?? 0)), MidpointRounding.AwayFromZero)
                    //    };
                    //}
                    //else
                    //    usage.AddCalculation(new UsageCalculation
                    //    {
                    //        Units = 0 - invoices.Sum(ii => ii.GetUsage(zoneNumber).Units),
                    //        Charge = 0 - invoices.Sum(ii => ii.GetUsage(zoneNumber).Charge),
                    //        Discount = 0 - invoices.Sum(ii => ii.GetUsage(zoneNumber).Discount),
                    //        DiscountUnits = 0 - invoices.Sum(ii => ii.GetUsage(zoneNumber).DiscountUnits),
                    //        PriceValue = minTariffRate.Value
                    //    });
                

                usage.Units = usage.Calculations.Sum(c => c.Units);
                usage.Charge = usage.Calculations.Sum(c => c.Charge);
                usage.Discount = usage.Calculations.Sum(c => c.Discount);
                usage.DiscountUnits = usage.Calculations.Sum(c => c.DiscountUnits);
            }

            void CalculateInternal(Expression<Func<Usage>> usageExpr, TariffRate tr)
            {
                var usage = usageExpr.Compile().Invoke();
                var usageName = ((MemberExpression)usageExpr.Body).Member.Name;
                var consumptionMonthLimit = GetConsumptionMonthLimitInternal(tr, FromDate, ToDate);
                var units = usage.Units - usage.Calculations.Sum(c => c.Units);
                if (units > consumptionMonthLimit)
                    units = usageName switch
                    {
                        "UsageT1" when ZoneRecord == ZoneRecord.None => consumptionMonthLimit - UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).Sum(c=>c.Units),
                        "UsageT2" when ZoneRecord == ZoneRecord.Two => consumptionMonthLimit - UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).Single().Units,
                        "UsageT3" => consumptionMonthLimit - UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).Single().Units + UsageT2.Calculations.Where(c => c.PriceValue == tr.Value).Single().Units,
                        _ => (int)decimal.Round(consumptionMonthLimit * usage.Units / (decimal)(UsageT1.Units + (UsageT2?.Units ?? 0) + (UsageT3?.Units ?? 0)), MidpointRounding.AwayFromZero)
                    };

                usage.AddCalculation(new UsageCalculation
                {
                    Units = units,
                    Charge = Math.Round(units * tr.Value * usage.Kz, 2, MidpointRounding.AwayFromZero),
                    Discount = decimal.Round(units * tr.Value * UsageT1.Kz * UsageT1.DiscountWeight, 2, MidpointRounding.AwayFromZero) * (ExemptionCoeff ?? 1),
                    DiscountUnits = 0,
                    PriceValue = tr.Value
                });
            }

            static int GetConsumptionMonthLimitInternal(TariffRate rate, DateTime fromDate, DateTime toDate)
            {
                if (rate.HeatingConsumptionLimit.HasValue)
                {
                    var heatingStart = new DateTime(fromDate.Year, rate.HeatingStartDay.Value.Month, rate.HeatingStartDay.Value.Day);
                    var heatingEnd = new DateTime(toDate.Year, rate.HeatingEndDay.Value.Month, rate.HeatingEndDay.Value.Day);
                    if (fromDate > heatingStart || toDate < heatingEnd)
                        return rate.HeatingConsumptionLimit.Value;
                }

                return rate.ConsumptionLimit ?? int.MaxValue;
            }
        }

        public void Calculate_old()
        {
            if (Type == InvoiceType.Recalculation)
            {
                var tariffRates = Tariff.Rates.Where(t => t.StartDate <= FromDate).GroupBy(tr => tr.StartDate).OrderByDescending(tr => tr.Key).First();

                foreach (var tr in tariffRates.OrderBy(tr => tr.ConsumptionLimit ?? int.MaxValue))
                {
                    if (Math.Abs(UsageT1.Calculations.Sum(c => c.DiscountUnits)) < Math.Abs(UsageT1.DiscountUnits))
                    {
                        CalculateDiscount(() => UsageT1, tr);
                        if (UsageT2 != null) CalculateDiscount(() => UsageT2, tr);
                        if (UsageT3 != null) CalculateDiscount(() => UsageT3, tr);
                    }
                }

                TotalDiscount = UsageT1.Discount + (UsageT2?.Discount ?? 0) + (UsageT3?.Discount ?? 0);
                TotalCharge = UsageT1.Charge + (UsageT2?.Charge ?? 0) + (UsageT3?.Charge ?? 0);
                TotalAmountDue = TotalCharge - TotalDiscount;
                IncomingBalance = AccountingPoint.Debt;
            }
            else
            {

            }

            void CalculateDiscount(Expression<Func<Usage>> usageExpr, TariffRate tr)
            {
                var zoneNumber = Convert.ToInt32(((MemberExpression)usageExpr.Body).Member.Name.Substring(6));
                var usage = usageExpr.Compile().Invoke();
                var units = usage.DiscountUnits;
                if (units == 0) return;

                if (tr.ConsumptionLimit.HasValue)
                {
                    var limit = zoneNumber switch
                    {
                        2 when UsageT3 is null => tr.ConsumptionLimit.Value - Math.Abs(UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).FirstOrDefault().DiscountUnits),
                        3 => tr.ConsumptionLimit.Value - Math.Abs(UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).FirstOrDefault().DiscountUnits + UsageT2.Calculations.Where(c => c.PriceValue == tr.Value).FirstOrDefault().DiscountUnits),
                        _ => (int)decimal.Round((tr.ConsumptionLimit.Value) * usage.DiscountUnits / (decimal)(UsageT1.DiscountUnits + (UsageT2?.DiscountUnits ?? 0) + (UsageT3?.DiscountUnits ?? 0)), MidpointRounding.AwayFromZero)
                    };

                    if (Math.Abs(usage.DiscountUnits) > limit)
                        units = limit;
                }

                usage.AddCalculation(new UsageCalculation
                {
                    Charge = 0,
                    Discount = decimal.Round(units * tr.Value * UsageT1.Kz * UsageT1.DiscountWeight, 2, MidpointRounding.AwayFromZero) * (ExemptionCoeff ?? 1),
                    DiscountUnits = units,
                    PriceValue = tr.Value
                });
            }
        }

        
    }
}
