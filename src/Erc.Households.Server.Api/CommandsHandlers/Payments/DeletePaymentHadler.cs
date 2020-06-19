using Erc.Households.Api.Queries.Payments;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.Payments
{
    public class DeletePaymentHandler : IRequestHandler<DeletePayment, bool>
    {
        private readonly ErcContext _ercContext;

        public DeletePaymentHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<bool> Handle(DeletePayment request, CancellationToken cancellationToken)
        {
            var payment = await _ercContext.Payments.FindAsync(request.Id);
            _ercContext.Remove(payment);

            return true;
        }
    }
}
