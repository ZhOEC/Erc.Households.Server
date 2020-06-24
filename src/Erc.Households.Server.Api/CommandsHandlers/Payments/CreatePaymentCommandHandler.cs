using Erc.Households.Api.Queries.Payments;
using Erc.Households.Domain.Payments;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.Payments
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Unit>
    {
        private readonly ErcContext _ercContext;

        public CreatePaymentCommandHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<Unit> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var accountingPoint = await _ercContext.AccountingPoints.Include(x => x.BranchOffice).FirstOrDefaultAsync(x => x.Id == request.Payment.AccountingPointId);
            if (accountingPoint is null)
                throw new Exception("Accounting point not found");

            var payment = new Payment(
                    request.Payment.PayDate,
                    request.Payment.Amount,
                    accountingPoint.BranchOffice.CurrentPeriodId,
                    request.Payment.Type,
                    request.Payment.PayerInfo,
                    request.Payment.AccountingPointId,
                    null,
                    request.Payment.BatchId
                );

            await _ercContext.Payments.AddAsync(payment);
            return await _ercContext.SaveChangesAsync() > 0 ? Unit.Value : throw new Exception("Can't create payment");
        }
    }
}
