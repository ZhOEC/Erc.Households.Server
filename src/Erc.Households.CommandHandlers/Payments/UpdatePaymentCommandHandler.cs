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
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, Unit>
    {
        private readonly ErcContext _ercContext;

        public UpdatePaymentCommandHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<Unit> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _ercContext.Payments.Include(t => t.AccountingPoint).FirstOrDefaultAsync(x => x.Id == request.Id);

            if (payment is null)
                throw new Exception("Payment not exist");
            else if (payment.Status == PaymentStatus.Processed)
                throw new Exception("The payment has been made and cannot be edited");

            _ercContext.Entry(payment).CurrentValues.SetValues(request);

            return Unit.Value;
        }
    }
}
