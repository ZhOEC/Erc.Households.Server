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
            //var invoices = await _ercContext.Invoices
            //    .Where(x => x.PeriodId == request.PeriodId)
            //    .ToListAsync();
            //var weightedAvgTariff = invoices.Sum(x => x.TotalAmountDue) / invoices.Sum(x => x.TotalUnits);

            //var kfk = 0;//_ercContext.KFKPayments.Where(x => x.PeriodId == previousPeriod.Id && x.).SumAsync(k=>k.Sum);
            //var consumerAndSocialPayments = await _ercContext.Payments.Include(p => p.AccountingPoint)
            //    .Where(f => f.Status == PaymentStatus.Processed
            //        && f.PeriodId == request.PeriodId && request.BranchOfficeId == f.AccountingPoint.BranchOfficeId
            //        && (f.Type == PaymentType.CustomerPayment || f.Type == PaymentType.SocialHelp))
            //    .SumAsync(p => p.Amount);

            //var consumerTaxInvoice = new TaxInvoice
            //{
            //    LiabilityDate = request.PeriodEndDate,
            //    Type = TaxInvoiceType.Electricity,
            //    EnergyAmount = consumerAndSocialPayments,
            //    TariffValue = weightedAvgTariff,
            //    FullSum = decimal.Round((consumerAndSocialPayments + kfk) * weightedAvgTariff, 2),
            //    LiabilitySum = decimal.Round((consumerAndSocialPayments + kfk) * weightedAvgTariff * (decimal)0.2, 2),
            //    CreationDate = DateTime.Now
            //};


            ///* Compensation Tax Invoice */
            //var compensationPayments = await _ercContext.Payments
            //    .Where(f => f.Status == PaymentStatus.Processed
            //        && f.PeriodId == request.PeriodId
            //        && f.Type == PaymentType.DsoCompensation)
            //    .ToListAsync();

            //var compensationTaxInvoice = new TaxInvoice
            //{
            //    LiabilityDate = compensationPayments.First().PayDate,
            //    Type = TaxInvoiceType.Electricity,
            //    EnergyAmount = compensationPayments.Sum(x => x.Amount),
            //    TariffValue = 1,
            //    FullSum = decimal.Round(compensationPayments.Sum(x => x.Amount), 2),
            //    LiabilitySum = decimal.Round(compensationPayments.Sum(x => x.Amount) * (decimal)0.2, 2),
            //    CreationDate = DateTime.Now,
            //};

            //_ercContext.TaxInvoices.Add(consumerTaxInvoice);
            //if (compensationTaxInvoice.FullSum > 0)
            //    _ercContext.TaxInvoices.Add(compensationTaxInvoice);

        }
    }
}
