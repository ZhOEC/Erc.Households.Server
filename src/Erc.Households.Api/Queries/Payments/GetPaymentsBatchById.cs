using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class GetPaymentsBatchById : IRequest<Responses.PaymentsBatch>
    {
        public int Id { get; private set; }

        public GetPaymentsBatchById (int id)
        {
            Id = id;
        }
    }
}
