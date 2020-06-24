using Erc.Households.BranchOfficeManagment.Core;
using Erc.Households.Domain.Billing;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.NotificationHandlers
{
    public class AccountingPointExemptionClosedHandler : INotificationHandler<AccountingPointExemptionClosed>
    {
        private readonly ErcContext _ercContext;
        private readonly IBranchOfficeService _branchOfficeService;

        public AccountingPointExemptionClosedHandler(ErcContext ercContext, IBranchOfficeService branchOfficeService)
        {
            _ercContext = ercContext;
            _branchOfficeService = branchOfficeService;
        }

        public async Task Handle(AccountingPointExemptionClosed notification, CancellationToken cancellationToken)
        {
            var ac = await _ercContext.AccountingPoints.FindAsync(notification.AccountingPointId);
            var currentPeriodId = _branchOfficeService.GetOne(ac.BranchOfficeId).CurrentPeriodId;
            var invoiceFactory = new Dictionary<int, Func<Invoice, DateTime, DateTime, decimal, Invoice>>()
            {
                {
                    1, (inv, fromDate, toDate, k) =>
                    {
                        var usageT1 = MakeCorrectiveUsage(inv.UsageT1, k);
                        return  new Invoice(currentPeriodId, fromDate, toDate, inv.TariffId, usageT1, InvoiceType.Recalculation, note: "Скасування пільги");
                    }
                },

                {
                    2, (inv, fromDate, toDate, k) =>
                    {
                        var usageT1 = MakeCorrectiveUsage(inv.UsageT1, k);
                        var usageT2 = MakeCorrectiveUsage(inv.UsageT2, k);

                        return  new Invoice(currentPeriodId, fromDate, toDate, inv.TariffId, usageT1, usageT2, InvoiceType.Recalculation, note: "Скасування пільги");
                    }
                },

                {
                    3, (inv, fromDate, toDate, k) =>
                    {
                        var usageT1 = MakeCorrectiveUsage(inv.UsageT1, k);
                        var usageT2 = MakeCorrectiveUsage(inv.UsageT2, k);
                        var usageT3 = MakeCorrectiveUsage(inv.UsageT3, k);

                        return  new Invoice(currentPeriodId, fromDate, toDate, inv.TariffId, usageT1, usageT2, usageT3, InvoiceType.Recalculation, note: "Скасування пільги");
                    }
                }
            };

            var invoices = await _ercContext.Invoices.Where(i => i.AccountingPointId == notification.AccountingPointId && i.ToDate > notification.Date.AddDays(1)).ToArrayAsync();
            
            foreach (var inv in invoices)
            {
                var fromDate = (inv.FromDate <= notification.Date && inv.ToDate > notification.Date) ? notification.Date.AddDays(1) : inv.FromDate;
                var k = (decimal)(inv.ToDate - fromDate).Days / (inv.ToDate - inv.FromDate).Days;
                var newInvoice = invoiceFactory[inv.ZoneCount](inv, fromDate, inv.ToDate, k);
                _ercContext.Entry(newInvoice).State = EntityState.Added;
                ac.AddInvoice(newInvoice);
            }

            static Usage MakeCorrectiveUsage(Usage existingUsage, decimal k)
                => new Usage
                {
                    Units = 0,
                    DiscountUnits = 0 - (int)decimal.Round(existingUsage.DiscountUnits * k, MidpointRounding.AwayFromZero),
                    Charge = 0,
                    Discount = 0 - decimal.Round(existingUsage.Discount * k, 2, MidpointRounding.AwayFromZero),
                    DiscountWeight = existingUsage.DiscountWeight,
                    Kz = existingUsage.Kz
                };
        }
    }
}
