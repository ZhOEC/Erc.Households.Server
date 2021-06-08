using Erc.Households.CalculateStrategies.Core;
using Erc.Households.Domain.Shared;
using Erc.Households.Domain.Shared.Billing;
using Erc.Households.Domain.Shared.Tariffs;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Erc.Households.CalculateStrategies.ElectricPower
{
    public class ElectricPowerCalculateStrategy : ICalculateStrategy
    {
        private static DateTime GetHatingSeasonStartDate(int year) => new(year, 10, 16);
        private static DateTime GetHatingSeasonEndDate(int year) => new(year, 4, 15);

        IEnumerable<(Usage invalidUsageT1, Usage invalidUsageT2, Usage invalidUsageT3)> _invalidUsages;
        Func<int, DateTime, ZoneRecord, Task<IEnumerable<(Usage invalidUsageT1, Usage invalidUsageT2, Usage invalidUsageT3)>>> _invalidUsageLoader;
       
        public ElectricPowerCalculateStrategy(Func<int, DateTime, ZoneRecord, Task<IEnumerable<(Usage invalidUsageT1, Usage invalidUsageT2, Usage invalidUsageT3)>>> invalidUsageLoader)
        {
            _invalidUsageLoader = invalidUsageLoader;
        }

        public async Task Calculate(CalculationRequest calculationRequest)
        {
            Log.Debug($"Start processing calculation request: {calculationRequest}");
            if (calculationRequest.InvoiceType == InvoiceType.Recalculation)
            {
                _invalidUsages = await _invalidUsageLoader?.Invoke(calculationRequest.AccountingPointId, calculationRequest.FromDate, calculationRequest.ZoneRecord);
                foreach (var usages in _invalidUsages)
                {
                    calculationRequest.UsageT1.AddInvalidInvoicesUnits(usages.invalidUsageT1.Units);
                    if (usages.invalidUsageT2 is not null) calculationRequest.UsageT2?.AddInvalidInvoicesUnits(usages.invalidUsageT2.Units);
                    if (usages.invalidUsageT3 is not null) calculationRequest.UsageT3?.AddInvalidInvoicesUnits(usages.invalidUsageT3.Units);
                }
            }

            var tariffRates = calculationRequest.Tariff.Rates
                .Where(t => t.StartDate < calculationRequest.ToDate)
                .GroupBy(tr => tr.StartDate)
                .OrderByDescending(tr => tr.Key)
                .First();

            foreach (var tr in tariffRates.OrderBy(tr => tr.ConsumptionLimit ?? int.MaxValue))
            {
                if (calculationRequest.UsageT1.Units > calculationRequest.UsageT1.Calculations.Sum(c => c.Units)) CalculateUsageInternal(() => calculationRequest.UsageT1, tr);
                if (calculationRequest.UsageT2?.Units > calculationRequest.UsageT2?.Calculations.Sum(c => c.Units)) CalculateUsageInternal(() => calculationRequest.UsageT2, tr);
                if (calculationRequest.UsageT3?.Units > calculationRequest.UsageT3?.Calculations.Sum(c => c.Units)) CalculateUsageInternal(() => calculationRequest.UsageT3, tr);
            }

            if (_invalidUsages?.Any() ?? false)
            {
                foreach (var calculation in _invalidUsages.SelectMany(ii => ii.invalidUsageT1.Calculations).Where(c => c.Units != 0))
                {
                    calculationRequest.UsageT1.AddCalculation(new UsageCalculation
                    {
                        Charge = -calculation.Charge,
                        Discount = -calculation.Discount,
                        DiscountUnits = -calculation.DiscountUnits,
                        PriceValue = calculation.PriceValue,
                        Units = -calculation.Units
                    }, true);
                }

                if (calculationRequest.ZoneRecord != ZoneRecord.None)
                {
                    var u2 = _invalidUsages.Select(ii => ii.invalidUsageT2);
                    foreach (var calculation in _invalidUsages.Where(ii=>ii.invalidUsageT2 is not null).SelectMany(ii => ii.invalidUsageT2?.Calculations))
                    {
                        calculationRequest.UsageT2.AddCalculation(new UsageCalculation
                        {
                            Charge = -calculation.Charge,
                            Discount = -calculation.Discount,
                            DiscountUnits = -calculation.DiscountUnits,
                            PriceValue = calculation.PriceValue,
                            Units = -calculation.Units
                        }, true);
                    }

                    if (calculationRequest.ZoneRecord == ZoneRecord.Three)
                        foreach (var calculation in _invalidUsages.Where(ii => ii.invalidUsageT3 is not null).SelectMany(ii => ii.invalidUsageT3?.Calculations))
                        {
                            calculationRequest.UsageT3.AddCalculation(new UsageCalculation
                            {
                                Charge = -calculation.Charge,
                                Discount = -calculation.Discount,
                                DiscountUnits = -calculation.DiscountUnits,
                                PriceValue = calculation.PriceValue,
                                Units = -calculation.Units
                            }, true);
                        }
                }
            }

            void CalculateUsageInternal(Expression<Func<Usage>> usageExpr, TariffRate tr)
            {
                var usage = usageExpr.Compile().Invoke();
                var zoneName = ((MemberExpression)usageExpr.Body).Member.Name[5..];
                var consumptionMonthLimit = int.MaxValue;
                if (tr.ConsumptionLimit.HasValue || tr.HeatingConsumptionLimit.HasValue)
                    consumptionMonthLimit = GetConsumptionMonthLimit(tr, calculationRequest.FromDate, calculationRequest.ToDate);

                var units = usage.Units - usage.Calculations.Sum(c => c.Units);

                if (consumptionMonthLimit < int.MaxValue)
                {
                    var zoneLimit = zoneName switch
                    {
                        "T1" when calculationRequest.ZoneRecord == ZoneRecord.None => consumptionMonthLimit - calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).Sum(c => c.Units),
                        "T2" when calculationRequest.ZoneRecord == ZoneRecord.Two => consumptionMonthLimit - (calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).SingleOrDefault()?.Units??0),
                        "T3" => consumptionMonthLimit - ((calculationRequest.UsageT1.Calculations.Where(c => c.PriceValue == tr.Value).SingleOrDefault()?.Units ?? 0) + (calculationRequest.UsageT2.Calculations.Where(c => c.PriceValue == tr.Value).SingleOrDefault()?.Units ?? 0)),
                        _ => (int)decimal.Round(consumptionMonthLimit * (usage.Units / (calculationRequest.UsageT1.Units + (calculationRequest.UsageT2?.Units ?? 0) + (calculationRequest.UsageT3?.Units ?? 0))), MidpointRounding.AwayFromZero)
                    };

                    if (units > zoneLimit)
                        units = zoneLimit;
                }

                int? discountUnits = null;
                var discount = 0m;
                if (calculationRequest.ExemptionData != null)
                {
                    var exemptionConsumptionLimit = GetExemtionLimit(calculationRequest.ExemptionData, calculationRequest.FromDate, calculationRequest.ToDate);

                    if (exemptionConsumptionLimit < int.MaxValue)
                    {
                        discountUnits = (int)decimal.Round(exemptionConsumptionLimit * usage.DiscountWeight, MidpointRounding.AwayFromZero);
                        var alreadyDiscountedInUsage = usage.Calculations.Where(c => c.PriceValue != tr.Value).Sum(c => c.DiscountUnits);
                        discountUnits -= alreadyDiscountedInUsage;
                    }
                    if (discountUnits > units || !discountUnits.HasValue)
                        discountUnits = (int)units;

                    discount = decimal.Round(discountUnits.Value * tr.Value * usage.Kz * (calculationRequest.ExemptionData.ExemptionPercent > 1 ? (calculationRequest.ExemptionData.ExemptionPercent / 100) : calculationRequest.ExemptionData.ExemptionPercent), 2, MidpointRounding.AwayFromZero);
                }

                usage.AddCalculation(new UsageCalculation
                {
                    Units = units,
                    Charge = Math.Round(units * tr.Value * usage.Kz, 2, MidpointRounding.AwayFromZero),
                    Discount = discount,
                    DiscountUnits = discountUnits ?? 0,
                    PriceValue = tr.Value
                });
            }
        }

        static private int GetConsumptionMonthLimit(TariffRate rate, DateTime fromDate, DateTime toDate)
        {
            if (rate.HeatingConsumptionLimit.HasValue)
            {
                var heatingStart = new DateTime(fromDate.Year, rate.HeatingStartDay.Value.Month, rate.HeatingStartDay.Value.Day);
                var heatingEnd = new DateTime(toDate.Year, rate.HeatingEndDay.Value.Month, rate.HeatingEndDay.Value.Day);
                if (fromDate >= heatingStart || toDate <= heatingEnd)
                    return rate.HeatingConsumptionLimit.Value;
            }

            return rate.ConsumptionLimit ?? int.MaxValue;
        }

        private static int GetExemtionLimit(ExemptionData exemptionData, DateTime fromDate, DateTime toDate)
        {
            decimal limit = 0;
            var heatingSeasonStart = GetHatingSeasonStartDate(fromDate.Year);
            var heatingSeasonEnd = GetHatingSeasonEndDate(fromDate.Year);

            ExemptionDiscountNorms discountNorms = exemptionData.ExemptionDiscountNorms.OrderByDescending(n => n.EffectiveDate).First(n => n.EffectiveDate <= fromDate);
            if (exemptionData.IsElectricHeatig && (toDate>heatingSeasonStart || fromDate < heatingSeasonEnd))
            {
                limit = (discountNorms.SquareMeterPerPerson.Value * exemptionData.NumberOfPeople + discountNorms.BaseSquareMeter.Value) * discountNorms.UnitsPerSquareMeter.Value * exemptionData.HeatingCorrection;
                if (fromDate.Month == heatingSeasonStart.Month)
                    limit *= ((decimal)16) / 31;
                if (fromDate.Month == heatingSeasonEnd.Month)
                    limit *= 0.5m;

                limit = decimal.Round(limit, MidpointRounding.AwayFromZero);
            }
            else
            {
                if (exemptionData.UseDiscountLimit)
                {
                    var canBeUsedElectricWaterHeater = !exemptionData.IsCentralizedHotWaterSupply && !exemptionData.IsGasWaterHeaterInstalled && exemptionData.CanBeUsedElectricWaterHeater;
                    var maxLimit = canBeUsedElectricWaterHeater ? discountNorms.MaxUnitsWithoutHotWater : discountNorms.MaxUnits;
                    limit = (canBeUsedElectricWaterHeater ? discountNorms.BaseUnitsWithoutHotWater : discountNorms.BaseUnits) + (exemptionData.NumberOfPeople - discountNorms.BasePerson) * discountNorms.UnitsPerPerson;
                    if (limit > maxLimit)
                        limit = maxLimit;
                }
                else
                    limit = int.MaxValue;
            }
            return (int)limit;
        }
    }
}
