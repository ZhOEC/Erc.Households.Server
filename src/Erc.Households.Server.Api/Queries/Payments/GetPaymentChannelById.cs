using Erc.Households.Domain.Payments;
using MediatR;

namespace Erc.Households.Api.Queries.Payments
{
    public class GetPaymentChannelById : IRequest<PaymentChannel>
    {
        public int PaymentChannelId { get; set; }

        public GetPaymentChannelById(int paymentChannelId)
        {
            PaymentChannelId = paymentChannelId;
        }
    }
}
