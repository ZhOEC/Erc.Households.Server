using Erc.Households.Commands.Payments;
using Erc.Households.Domain.Payments;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.CommandHandlers.Payments
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
            var accountingPoint = await _ercContext.AccountingPoints.Include(x => x.BranchOffice).FirstOrDefaultAsync(x => x.Id == request.AccountingPointId);
            if (accountingPoint is null)
                throw new Exception("Accounting point not found");

            var payment = new Payment(
                    request.PayDate,
                    request.Amount,
                    accountingPoint.BranchOffice.CurrentPeriodId,
                    (PaymentType)request.Type,
                    request.PayerInfo,
                    request.AccountingPointId,
                    null,
                    request.BatchId
                );

            await _ercContext.Payments.AddAsync(payment);
            return Unit.Value;
        }
    }
}
