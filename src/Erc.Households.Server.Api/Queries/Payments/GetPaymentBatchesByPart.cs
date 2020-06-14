using MediatR;
using X.PagedList;

namespace Erc.Households.Api.Queries.Payments
{
    public class GetPaymentBatchesByPart : IRequest<IPagedList<Responses.PaymentsBatch>>
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public bool ShowClosed { get; private set; }

        public GetPaymentBatchesByPart(int pageNumber, int pageSize, bool showClosed)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            ShowClosed = showClosed;
        }
    }
}
