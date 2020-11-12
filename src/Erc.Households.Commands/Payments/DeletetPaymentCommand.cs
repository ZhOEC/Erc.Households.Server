using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class DeletePaymentCommand : IRequest
    {
        public DeletePaymentCommand(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}
