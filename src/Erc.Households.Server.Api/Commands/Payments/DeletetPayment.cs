using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class DeletePayment : IRequest<bool>
    {
        public int Id;

        public DeletePayment(int id)
        {
            Id = id;
        }
    }
}
