using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class DeletePaymentCommand : IRequest<Unit>
    {
        public DeletePaymentCommand(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}
