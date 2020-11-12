using Erc.Households.Api.Queries.Payments;
using Erc.Households.Domain.Payments;
using Erc.Households.EF.PostgreSQL;
using MediatR;
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
            var payment = await _ercContext.Payments.FindAsync(request.Id);

            if (payment is null)  return;

            if (payment.Status == PaymentStatus.Processed)
                throw new Exception("The payment has been processed and cannot be removed");

            _ercContext.Remove(payment);
        }
    }
}
