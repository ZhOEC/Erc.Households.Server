using Erc.Households.Domain.Taxes;
using MediatR;

namespace Erc.Households.Api.Queries.TaxInvoices
{
    public class GetTaxInvoiceByPeriodId : IRequest<TaxInvoice>
    {
        public int BranchOfficeId { get; private set; }
        public int PeriodId { get; private set; }

        public GetTaxInvoiceByPeriodId(int branchOfficeId, int periodId)
        {
            BranchOfficeId = branchOfficeId;
            PeriodId = periodId;
        }
    }
}
