using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class DeletePaymentCommand : IRequest<Unit>
    {
        public int Id { get; private set; }

        public DeletePaymentCommand(int id)
        {
            Id = id;
        }
    }
}
