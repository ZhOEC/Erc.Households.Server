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

            var usageT1 = new Usage(context.Message.UsageT1,
                zoneRecord switch
                {
                    ZoneRecord.Two => 0.5m,
                    ZoneRecord.Three => 0.4m,
                    _ => 1
                })

            {
                PresentMeterReading = context.Message.PresentMeterReadingT1,
                PreviousMeterReading = context.Message.PreviousMeterReadingT1,
            };

            var usageT2 = zoneRecord != ZoneRecord.None ? new Usage(context.Message.UsageT2 ?? 0, 1)
            {
                PresentMeterReading = context.Message.PresentMeterReadingT2,
                PreviousMeterReading = context.Message.PreviousMeterReadingT2,
            } : null;

            var usageT3 = zoneRecord == ZoneRecord.Three ? new Usage(context.Message.UsageT3 ?? 0, 1.5m)
            {
                PresentMeterReading = context.Message.PresentMeterReadingT3,
                PreviousMeterReading = context.Message.PreviousMeterReadingT3,
            } : null;

            var newInvoice = new Invoice(context.Message.Id, ac, fromDate, toDate, zoneRecord, usageT1, usageT2, usageT3);
            
            try
            {
                await newInvoice.CalculateAsync(calculateStrategy);
                ac.ApplyInvoice(newInvoice);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error calculating AccountingPoint {context.Message}");
                throw;
            }

            await _ercContext.SaveChangesAsync();
        }
    }
}
