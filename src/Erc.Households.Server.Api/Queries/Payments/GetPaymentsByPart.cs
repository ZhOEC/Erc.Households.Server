using MediatR;
using X.PagedList;

namespace Erc.Households.Api.Queries.Payments
{
    public class GetPaymentsByPart : IRequest<IPagedList<Responses.Payment>>
    {
        public int PaymentsBatchId { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public bool ShowProcessed { get; private set; }

        public GetPaymentsByPart(int paymentsBatchId, int pageNumber, int pageSize, bool showProcessed)
        {
            PaymentsBatchId = paymentsBatchId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            ShowProcessed = showProcessed;
        }
    }
}
