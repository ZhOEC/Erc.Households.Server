using Erc.Households.Api.Requests;
using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class UpdatePayment : IRequest<Responses.Payment>
    {
        public UpdatedPayment UpdatedPayment { get; private set; }

        public UpdatePayment(UpdatedPayment updatedPayment)
        {
            UpdatedPayment = updatedPayment;
        }
    }
}
