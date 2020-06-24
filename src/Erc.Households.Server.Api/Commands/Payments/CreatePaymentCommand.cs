using Erc.Households.Api.Requests;
using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class CreatePaymentCommand : IRequest<Unit>
    {
        public NewPayment Payment { get; private set; }

        public CreatePaymentCommand(NewPayment payment)
        {
            Payment = payment;
        }
    }
}
