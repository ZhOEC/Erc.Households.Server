using Erc.Households.Domain.Taxes;
using MediatR;
using System.Collections.Generic;

namespace Erc.Households.Api.Queries.TaxInvoices
{
    public class GetTaxInvoicesByPeriodId : IRequest<IEnumerable<TaxInvoice>>
    {
        public int BranchOfficeId { get; private set; }
        public int PeriodId { get; private set; }

        public GetTaxInvoicesByPeriodId(int branchOfficeId, int periodId)
        {
            BranchOfficeId = branchOfficeId;
            PeriodId = periodId;
        }
    }
}
