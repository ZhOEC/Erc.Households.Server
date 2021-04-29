using Erc.Households.Api.Queries.TaxInvoices;
using Erc.Households.Domain.Taxes;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers
{
    public class GetTaxInvoicesByPeriodIdHandler : IRequestHandler<GetTaxInvoicesByPeriodId, IEnumerable<TaxInvoice>>
    {
        private readonly ErcContext _ercContext;

        public GetTaxInvoicesByPeriodIdHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<IEnumerable<TaxInvoice>> Handle(GetTaxInvoicesByPeriodId request, CancellationToken cancellationToken)
            => await _ercContext.TaxInvoices.Where(x => x.BranchOfficeId == request.BranchOfficeId && x.PeriodId == request.PeriodId).ToListAsync();
    }
}
