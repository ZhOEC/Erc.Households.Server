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
    public class DeletePaymentHandler : AsyncRequestHandler<DeletePaymentCommand>
    {
        private readonly ErcContext _ercContext;

        public DeletePaymentHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        protected override async Task Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _ercContext.Payments.Include(p => p.AccountingPoint.BranchOffice).FirstOrDefaultAsync(p => p.Id == request.Id);

            if (payment is null)
                throw new Exception("Payment not exist");

            if (payment.Status == PaymentStatus.Processed && payment.AccountingPoint.BranchOffice.CurrentPeriodId == payment.PeriodId)
                payment.AccountingPoint.RemovePayment(payment);

            _ercContext.Remove(payment);
        }
    }
}
