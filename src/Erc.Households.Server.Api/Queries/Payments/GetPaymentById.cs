using Erc.Households.Api.Responses;
using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class GetPaymentById : IRequest<Payment>
    {
        public int Id { get; private set; }

        public GetPaymentById (int id)
        {
            Id = id;
        }
    }
}
