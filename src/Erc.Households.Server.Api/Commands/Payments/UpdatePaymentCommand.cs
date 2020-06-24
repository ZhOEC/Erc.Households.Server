using Erc.Households.Api.Requests;
using Erc.Households.Domain.Payments;
using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class UpdatePaymentCommand : IRequest<Unit>
    {
        public UpdatedPayment UpdatedPayment { get; private set; }

        public UpdatePaymentCommand(UpdatedPayment updatedPayment)
        {
            UpdatedPayment = updatedPayment;
        }
    }
}
