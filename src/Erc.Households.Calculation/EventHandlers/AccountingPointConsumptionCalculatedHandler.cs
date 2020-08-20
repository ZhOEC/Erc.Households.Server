
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Billing;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Events.AccountingPoints;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Calculation.EventHandlers
{
    public class AccountingPointConsumptionCalculatedHandler : IConsumer<ConsumptionCalculated>
    {
        readonly ErcContext _ercContext;

        public AccountingPointConsumptionCalculatedHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task Consume(ConsumeContext<ConsumptionCalculated> context)
        {
            if (await _ercContext.Invoices.AnyAsync(i => i.DsoConsumptionId == context.Message.Id))
                return;

            var ac = await _ercContext.AccountingPoints
                .Include(a => a.Exemptions)
                    .ThenInclude(e => e.Category)
                .Include(a => a.TariffsHistory)
                    .ThenInclude(t => t.Tariff.Rates)
                .FirstOrDefaultAsync(a => a.Eic == context.Message.Eic);

            if (ac is null)
            {
                throw new ArgumentOutOfRangeException("Accountingpoint not found in the database!");
            }
            else
            {
                var tariff = ac.TariffsHistory.OrderByDescending(t => t.StartDate).FirstOrDefault(t => t.StartDate <= context.Message.FromDate).Tariff;
                var zoneRecord = context.Message.ZoneRecord == 0 ? ZoneRecord.None : (ZoneRecord)context.Message.ZoneRecord;
                var zcoeffs = _ercContext.ZoneCoeffs.ToArray();
                var exemption = ac.Exemptions.OrderByDescending(e => e.EffectiveDate).FirstOrDefault(e => e.EffectiveDate <= context.Message.FromDate && (e.EndDate ?? DateTime.MaxValue) > context.Message.FromDate);

                var usageT1 = new Usage
                {
                    PresentMeterReading = context.Message.PresentMeterReadingT1,
                    PreviousMeterReading = context.Message.PreviousMeterReadingT1,
                    Units = context.Message.UsageT1,
                    Kz = zcoeffs.OrderByDescending(zc=>zc.StartDate).First(zc => zc.ZoneNumber == ZoneNumber.T1 && zc.ZoneRecord == zoneRecord && zc.StartDate<=context.Message.FromDate).Value
                };

                var usageT2 = context.Message.UsageT2.HasValue ? new Usage
                {
                    PresentMeterReading = context.Message.PresentMeterReadingT2,
                    PreviousMeterReading = context.Message.PreviousMeterReadingT2,
                    Units = context.Message.UsageT2.Value,
                    Kz = zcoeffs.OrderByDescending(zc => zc.StartDate).First(zc => zc.ZoneNumber == ZoneNumber.T2 && zc.ZoneRecord == zoneRecord && zc.StartDate <= context.Message.FromDate).Value
                } : null;

                var usageT3 = context.Message.UsageT3.HasValue ? new Usage
                {
                    PresentMeterReading = context.Message.PresentMeterReadingT3,
                    PreviousMeterReading = context.Message.PreviousMeterReadingT3,
                    Units = context.Message.UsageT3.Value,
                    Kz = zcoeffs.OrderByDescending(zc => zc.StartDate).First(zc => zc.ZoneNumber == ZoneNumber.T3 && zc.ZoneRecord == zoneRecord && zc.StartDate <= context.Message.FromDate).Value
                } : null;


                 await _ercContext.Invoices
                    .Where(i => i.AccountingPointId == ac.Id && i.FromDate.Month == context.Message.FromDate.Month && i.FromDate.Year == context.Message.FromDate.Year)
                    .ToArrayAsync()
                    .ContinueWith(t=>
                    {
                        var newInvoice = new Invoice(context.Message.Id, context.Message.PeriodId, ac.Debt,
                                              context.Message.FromDate, context.Message.ToDate,
                                              context.Message.MeterNumber, tariff, usageT1, usageT2, usageT3, exemption);

                        newInvoice.Calculate(t.Result);
                        ac.AddInvoice(newInvoice);
                    });

                await _ercContext.SaveChangesAsync();
            }
        }

        
    }
}
