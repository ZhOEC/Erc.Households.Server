using Erc.Households.Commands;
using Erc.Households.Domain.Payments;
using Erc.Households.Domain.Taxes;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.CommandHandlers
{
    public class CreateTaxInvoiceCommandHandler : AsyncRequestHandler<CreateTaxInvoiceCommand>
    {
        private readonly ErcContext _ercContext;

        public CreateTaxInvoiceCommandHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        protected override async Task Handle(CreateTaxInvoiceCommand request, CancellationToken cancellationToken)
        {
            var previousPeriod = await _ercContext.Periods.LastOrDefaultAsync(x => x.Id < request.CurrentPeriod.Id);

            if (previousPeriod != null)
            {
                var invoices = await _ercContext.Invoices
                    .Where(x => x.PeriodId == previousPeriod.Id)
                    .ToListAsync();
                var weightedAvgTariff = invoices.Sum(x => x.TotalAmountDue) / invoices.Sum(x => x.TotalUnits);

                var kfk = _ercContext.KFKPayments.FirstOrDefault(x => x.PeriodId == previousPeriod.Id);
                var consumerAndSocialPayments = await _ercContext.Payments
                    .Where(f => f.Status == PaymentStatus.Processed
                        && f.PeriodId == previousPeriod.Id
                        && (f.Type == PaymentType.CustomerPayment || f.Type == PaymentType.SocialHelp))
                    .ToListAsync();

                var consumerTaxInvoice = new TaxInvoice
                {
                    LiabilityDate = previousPeriod.EndDate,
                    Type = TaxInvoiceType.Electricity,
                    EnergyAmount = consumerAndSocialPayments.Sum(x => x.Amount),
                    TariffValue = weightedAvgTariff,
                    FullSum = decimal.Round((consumerAndSocialPayments.Sum(x => x.Amount) + kfk.Sum) * weightedAvgTariff, 2),
                    LiabilitySum = decimal.Round((consumerAndSocialPayments.Sum(x => x.Amount) + kfk.Sum)  * weightedAvgTariff * (decimal)0.2, 2),
                    CreationDate = DateTime.Now
                };

                
                /* Compensation Tax Invoice */
                var compensationPayments = await _ercContext.Payments
                    .Where(f => f.Status == PaymentStatus.Processed
                        && f.PeriodId == previousPeriod.Id
                        && f.Type == PaymentType.DsoCompensation)
                    .ToListAsync();

                var compensationTaxInvoice = new TaxInvoice
                {
                    LiabilityDate = compensationPayments.First().PayDate,
                    Type = TaxInvoiceType.Electricity,
                    EnergyAmount = compensationPayments.Sum(x => x.Amount),
                    TariffValue = 1,
                    FullSum = decimal.Round(compensationPayments.Sum(x => x.Amount), 2),
                    LiabilitySum = decimal.Round(compensationPayments.Sum(x => x.Amount) * (decimal)0.2, 2),
                    CreationDate = DateTime.Now,
                };

                _ercContext.TaxInvoices.Add(consumerTaxInvoice);
                if (compensationTaxInvoice.FullSum > 0)
                    _ercContext.TaxInvoices.Add(compensationTaxInvoice);

                await _ercContext.SaveChangesAsync();
            }
        }
    }
}
