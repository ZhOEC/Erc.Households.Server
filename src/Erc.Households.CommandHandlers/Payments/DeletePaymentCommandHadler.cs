using Erc.Households.Api.Queries.Payments;
using Erc.Households.Domain.Payments;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.Payments
{
    public class DeletePaymentHandler : IRequestHandler<DeletePaymentCommand, Unit>
    {
        private readonly ErcContext _ercContext;

        public DeletePaymentHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<Unit> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _ercContext.Payments.FindAsync(request.Id);

            if (payment is null)
                throw new Exception("Payment not exist");
            else if (payment.Status == PaymentStatus.Processed)
                throw new Exception("The payment has been made and cannot be removed");

            _ercContext.Remove(payment);
            return Unit.Value;
        }
    }
}
