using Erc.Households.Api.Requests;
using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class AddPayment : IRequest<Responses.Payment>
    {
        public NewPayment payment;

        public AddPayment(NewPayment payment)
        {
            this.payment = payment;
        }
    }
}
