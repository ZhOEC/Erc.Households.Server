using MediatR;
using X.PagedList;

namespace Erc.Households.Api.Queries.Payments
{
    public class GetPaymentsByAccountingPoint : IRequest<IPagedList<Responses.AccountingPointPaymentListItem>>
    {
        public int AccountingPointId { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
     

        public GetPaymentsByAccountingPoint(int accountingPointId, int pageNumber, int pageSize)
        {
            AccountingPointId = accountingPointId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
