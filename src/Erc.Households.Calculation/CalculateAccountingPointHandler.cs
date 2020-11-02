
using Erc.Households.CalculateStrategies.Core;
using Erc.Households.CalculateStrategies.NaturalGas;
using Erc.Households.Commands;
using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Shared;
using Erc.Households.EF.PostgreSQL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
                .Include(a => a.Exemptions)
                    .ThenInclude(e => e.Category)
                .Include(a => a.TariffsHistory)
                    .ThenInclude(t => t.Tariff)
                .FirstOrDefaultAsync(a => a.Eic == context.Message.Eic);
            
            if (ac is null)
                throw new ArgumentOutOfRangeException("Accountingpoint not found in the database!");

            ICalculateStrategy calculateStrategy = ac.Commodity == Commodity.NaturalGas ? _serviceProvider.GetRequiredService<GasCalculateStrategy>() : throw new NotImplementedException();

            var fromDate = context.Message.FromDate == DateTime.MinValue ? ac.BranchOffice.CurrentPeriod.StartDate : context.Message.FromDate;
            var toDate = context.Message.ToDate == DateTime.MinValue ? ac.BranchOffice.CurrentPeriod.EndDate.AddDays(1) : context.Message.ToDate;
            var tariff = ac.TariffsHistory.OrderByDescending(t => t.StartDate).FirstOrDefault(t => t.StartDate <= fromDate).Tariff;

            var usageT1 = new Usage
            {
                PresentMeterReading = context.Message.PresentMeterReadingT1,
                PreviousMeterReading = context.Message.PreviousMeterReadingT1,
                Units = context.Message.UsageT1,
            };

            var usageT2 = context.Message.UsageT2.HasValue ? new Usage
            {
                PresentMeterReading = context.Message.PresentMeterReadingT2,
                PreviousMeterReading = context.Message.PreviousMeterReadingT2,
                Units = context.Message.UsageT2.Value,
            } : null;

            var usageT3 = context.Message.UsageT3.HasValue ? new Usage
            {
                PresentMeterReading = context.Message.PresentMeterReadingT3,
                PreviousMeterReading = context.Message.PreviousMeterReadingT3,
                Units = context.Message.UsageT3.Value,
            } : null;

            var newInvoice = new Invoice(context.Message.Id, ac.BranchOffice.CurrentPeriodId, ac.Debt, fromDate, toDate, 
                                        context.Message.MeterNumber, tariff, (ZoneRecord)context.Message.ZoneRecord, usageT1, usageT2, usageT3);
            
            newInvoice.Calculate(calculateStrategy);
            ac.AddInvoice(newInvoice);
          
           await _ercContext.SaveChangesAsync();

        }
    }
}
