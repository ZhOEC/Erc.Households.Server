﻿using Erc.Households.CalculateStrategies.Core;
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
                throw new ArgumentOutOfRangeException("Accounting point not found in the database!");

            ICalculateStrategy calculateStrategy = ac.Commodity == Commodity.NaturalGas ? _serviceProvider.GetRequiredService<GasCalculateStrategy>() : _serviceProvider.GetRequiredService<ElectricPowerCalculateStrategy>();

            var fromDate = context.Message.FromDate ?? ac.BranchOffice.CurrentPeriod.StartDate;
            var toDate = context.Message.ToDate ?? ac.BranchOffice.CurrentPeriod.EndDate.AddDays(1);
            var tariff = ac.TariffsHistory.OrderByDescending(t => t.StartDate).FirstOrDefault(t => t.StartDate < toDate).Tariff;

            var usageT1 = new Usage(context.Message.UsageT1,
                context.Message.ZoneRecord switch
                {
                    2 => 0.5m,
                    3 => 0.4m,
                    _ => 1
                })

            {
                PresentMeterReading = context.Message.PresentMeterReadingT1,
                PreviousMeterReading = context.Message.PreviousMeterReadingT1,
            };

            var usageT2 = context.Message.ZoneRecord > 1 ? new Usage(context.Message.UsageT2 ?? 0, 1)
            {
                PresentMeterReading = context.Message.PresentMeterReadingT2,
                PreviousMeterReading = context.Message.PreviousMeterReadingT2,
            } : null;

            var usageT3 = context.Message.ZoneRecord == 3 ? new Usage(context.Message.UsageT3 ?? 0, 1.5m)
            {
                PresentMeterReading = context.Message.PresentMeterReadingT3,
                PreviousMeterReading = context.Message.PreviousMeterReadingT3,
            } : null;

            var newInvoice = new Invoice(context.Message.Id, ac, fromDate, toDate, usageT1, usageT2, usageT3);

            await newInvoice.CalculateAsync(calculateStrategy);
            ac.ApplyInvoice(newInvoice);

            await _ercContext.SaveChangesAsync();
        }
    }
}
