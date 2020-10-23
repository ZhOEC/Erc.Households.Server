using MediatR;
using X.PagedList;

namespace Erc.Households.Api.Queries
{
    public class GetTaxInvoicesByPart : IRequest<IPagedList<Responses.TaxInvoice>>
    {
        public int BranchOfficeId { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }

        public GetTaxInvoicesByPart(int branchOfficeId, int pageNumber, int pageSize)
        {
            BranchOfficeId = branchOfficeId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
