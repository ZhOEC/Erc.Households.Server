using Erc.Households.CalculateStrategies.Core;
using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Shared;
using Erc.Households.Domain.Shared.Billing;
using Erc.Households.Domain.Shared.Tariffs;
using Erc.Households.EF.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Erc.Households.CalculateStrategies.ElectricPower
{
    public class ElectricPowerCalculateStrategy : ICalculateStrategy
    {
        readonly ErcContext _ercContext;
        IEnumerable<Invoice> _invalidInvoices;

        public ElectricPowerCalculateStrategy(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task Calculate(CalculationRequest calculationRequest)
        {
            if (calculationRequest.InvoiceType == InvoiceType.Recalculation)
                await AddInvalidInvoiceUnits(calculationRequest);

            var tariffRates = calculationRequest.Tariff.Rates
                .Where(t => t.StartDate < calculationRequest.ToDate)
                .GroupBy(tr => tr.StartDate)
                .OrderByDescending(tr => tr.Key)
                .First();

            foreach (var tr in tariffRates.OrderBy(tr => tr.ConsumptionLimit ?? int.MaxValue))
            {
                if (calculationRequest.UsageT1.Units > calculationRequest.UsageT1.Calculations.Sum(c => c.Units)) CalculateUsageInternal(calculationRequest.UsageT1, tr);
                if (calculationRequest.UsageT2?.Units > calculationRequest.UsageT2?.Calculations.Sum(c => c.Units)) CalculateUsageInternal(calculationRequest.UsageT2, tr);
                if (calculationRequest.UsageT3?.Units > calculationRequest.UsageT3?.Calculations.Sum(c => c.Units)) CalculateUsageInternal(calculationRequest.UsageT3, tr);
            }


            void CalculateUsageInternal(Usage usage, TariffRate tr)
            {
                var consumptionMonthLimit = GetConsumptionMonthLimitInternal(tr, calculationRequest.FromDate, calculationRequest.ToDate);
                var units = usage.Units - usage.Calculations.Sum(c => c.Units);
                if (units > consumptionMonthLimit)
                    units = usage.Zone switch
                    {
                        ZoneNumber.T1 when calculationRequest.ZoneRecord == ZoneRecord.None => consumptionMonthLimit - calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).Sum(c => c.Units),
                        ZoneNumber.T2 when calculationRequest.ZoneRecord == ZoneRecord.Two => consumptionMonthLimit - calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).Single().Units,
                        ZoneNumber.T3 => consumptionMonthLimit - calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).Single().Units + calculationRequest.UsageT2.Calculations.Where(c => c.PriceValue == tr.Value).Single().Units,
                        _ => (int)decimal.Round(consumptionMonthLimit * (usage.Units / (calculationRequest.UsageT1.Units + (calculationRequest.UsageT2?.Units ?? 0) + (calculationRequest.UsageT3?.Units ?? 0))), MidpointRounding.AwayFromZero)
                    };

                usage.AddCalculation(new UsageCalculation
                {
                    Units = units,
                    Charge = Math.Round(units * tr.Value * usage.Kz, 2, MidpointRounding.AwayFromZero),
                    //Discount = decimal.Round(units * tr.Value * UsageT1.Kz * UsageT1.DiscountWeight, 2, MidpointRounding.AwayFromZero) * (ExemptionCoeff ?? 1),
                    DiscountUnits = 0,
                    PriceValue = tr.Value
                });
            }
        }

        private async Task AddInvalidInvoiceUnits(CalculationRequest calculationRequest)
        {
            _invalidInvoices = await _ercContext.Invoices
                .Where(i => i.AccountingPointId == calculationRequest.AccountingPointId && i.FromDate == calculationRequest.FromDate)
                .ToArrayAsync();
            
            if (_invalidInvoices.Any())
            {
                calculationRequest.UsageT1.AddInvalidInvoicesUnits(_invalidInvoices.Sum(i => i.UsageT1.Units));
                if (calculationRequest.ZoneRecord != ZoneRecord.None) calculationRequest.UsageT2.AddInvalidInvoicesUnits(_invalidInvoices.Sum(i => i.UsageT2?.Units ?? 0));
                if (calculationRequest.ZoneRecord == ZoneRecord.Three) calculationRequest.UsageT3.AddInvalidInvoicesUnits(_invalidInvoices.Sum(i => i.UsageT3?.Units ?? 0));
            }
        }

        static private int GetConsumptionMonthLimitInternal(TariffRate rate, DateTime fromDate, DateTime toDate)
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
}
