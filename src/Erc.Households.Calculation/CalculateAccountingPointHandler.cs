using Erc.Households.CalculateStrategies.Core;
using Erc.Households.CalculateStrategies.ElectricPower;
using Erc.Households.CalculateStrategies.NaturalGas;
using Erc.Households.Commands;
using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Shared;
using Erc.Households.Domain.Shared.Billing;
using Erc.Households.EF.PostgreSQL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Calculation
{
    public class CalculateAccountingPointHandler : IConsumer<CalculateAccountingPoint>
    {
        readonly ErcContext _ercContext;
        IServiceProvider _serviceProvider;

        public CalculateAccountingPointHandler(ErcContext ercContext, IServiceProvider serviceProvider)
        {
            _ercContext = ercContext;
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<CalculateAccountingPoint> context)
        {
            if (await _ercContext.Invoices.AnyAsync(i => i.DsoConsumptionId == context.Message.Id))
                return;
            
            var ac = await _ercContext.AccountingPoints
                .Include(ap=>ap.BuildingType)
                .Include(ac => ac.BranchOffice.CurrentPeriod)
                .Include(ac=> ac.UsageCategory)
                .Include(a => a.Exemptions)
                    .ThenInclude(e => e.Category)
                .Include(a => a.TariffsHistory)
                    .ThenInclude(t => t.Tariff)
                .FirstOrDefaultAsync(a => a.Eic == context.Message.Eic);

            if (ac is null)
            {
                Log.Error($"Accounting point '{context.Message.Eic}' not found in the database!");
                throw new ArgumentOutOfRangeException($"Accounting '{context.Message.Eic}' point not found in the database!");
            }

            ICalculateStrategy calculateStrategy = ac.Commodity == Commodity.NaturalGas ? _serviceProvider.GetRequiredService<GasCalculateStrategy>() : _serviceProvider.GetRequiredService<ElectricPowerCalculateStrategy>();

            var fromDate = context.Message.FromDate ?? ac.BranchOffice.CurrentPeriod.StartDate;
            var toDate = context.Message.ToDate ?? ac.BranchOffice.CurrentPeriod.EndDate.AddDays(1);
            var tariff = ac.TariffsHistory.OrderByDescending(t => t.StartDate).FirstOrDefault(t => t.StartDate < toDate).Tariff;

            var zoneRecord = context.Message.ZoneRecord switch
            {
                2 => ZoneRecord.Two,
                3 => ZoneRecord.Three,
                _ => ZoneRecord.None
            };

            var coeffs = _ercContext.ZoneCoeffs.OrderByDescending(zc => zc.StartDate).Where(zc => zc.StartDate <= fromDate && zc.ZoneRecord == zoneRecord);
            
            var T1Coeffs = coeffs.First(zc => zc.ZoneNumber == ZoneNumber.T1);
            var usageT1 = new Usage(context.Message.UsageT1)
            {
                Kz = T1Coeffs.Value,
                DiscountWeight = T1Coeffs.DiscountWeight,
                PresentMeterReading = context.Message.PresentMeterReadingT1,
                PreviousMeterReading = context.Message.PreviousMeterReadingT1,
            };

            Usage usageT2 = null;
            Usage usageT3 = null;

            if (zoneRecord != ZoneRecord.None)
            {
                var T2Coeffs = coeffs.First(zc => zc.ZoneNumber == ZoneNumber.T2);
                usageT2 = new Usage(context.Message.UsageT2 ?? 0)
                {
                    Kz=T2Coeffs.Value, 
                    DiscountWeight = T2Coeffs.DiscountWeight,
                    PresentMeterReading = context.Message.PresentMeterReadingT2,
                    PreviousMeterReading = context.Message.PreviousMeterReadingT2,
                };

                if (zoneRecord == ZoneRecord.Three)
                {
                    var T3Coeffs = coeffs.First(zc => zc.ZoneNumber == ZoneNumber.T3);
                    usageT3 = new Usage(context.Message.UsageT3 ?? 0)
                    {
                        Kz = T3Coeffs.Value,
                        DiscountWeight = T3Coeffs.DiscountWeight,
                        PresentMeterReading = context.Message.PresentMeterReadingT3,
                        PreviousMeterReading = context.Message.PreviousMeterReadingT3,
                    };
                }
            }
            var newInvoice = new Invoice(context.Message.Id, ac, fromDate, toDate, zoneRecord, usageT1, usageT2, usageT3);
            
            try
            {
                await newInvoice.CalculateAsync(calculateStrategy);
                ac.AddInvoice(newInvoice);
                await _ercContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, "Error calculating AccountingPoint {@Message}", context.Message);
                throw;
            }
        }
    }
}
