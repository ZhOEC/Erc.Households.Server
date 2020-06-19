using MediatR;
using System.Collections.Generic;
using X.PagedList;

namespace Erc.Households.Api.Queries.Payments
{
    public class GetPaymentBatchesByPart : IRequest<IPagedList<Responses.PaymentsBatch>>
    {
        public IEnumerable<string> BranchOfficeStringIds { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public bool ShowClosed { get; private set; }

        public GetPaymentBatchesByPart(IEnumerable<string> branchOfficeStringIds, int pageNumber, int pageSize, bool showClosed)
        {
            BranchOfficeStringIds = branchOfficeStringIds;
            PageNumber = pageNumber;
            PageSize = pageSize;
            ShowClosed = showClosed;
        }
    }
}
