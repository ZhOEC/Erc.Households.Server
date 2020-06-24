using MediatR;
using X.PagedList;

namespace Erc.Households.Api.Queries.Invoices
{
    public class GetInvoicesByAccountingPoint : IRequest<IPagedList<Responses.Invoice>>
    {
        public int AccountingPointId { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }

        public GetInvoicesByAccountingPoint(int accountingPointId, int pageNumber, int pageSize)
        {
            AccountingPointId = accountingPointId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
