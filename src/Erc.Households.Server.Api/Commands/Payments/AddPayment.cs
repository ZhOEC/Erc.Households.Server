using Erc.Households.Api.Requests;
using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class AddPayment : IRequest<Responses.Payment>
    {
        public NewPayment Payment { get; private set; }

        public AddPayment(NewPayment payment)
        {
            Payment = payment;
        }
    }
}
