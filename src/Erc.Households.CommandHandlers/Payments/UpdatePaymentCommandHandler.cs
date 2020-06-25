using Erc.Households.Api.Queries.Payments;
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

            _ercContext.Entry(payment).CurrentValues.SetValues(new { request.Id, request.BatchId, request.AccountingPointId, request.PayDate, 
                                                                        request.Amount, request.PayerInfo, request.Type });
            return await _ercContext.SaveChangesAsync() > 0 ? Unit.Value : throw new Exception("Can't update payment");
        }
    }
}
