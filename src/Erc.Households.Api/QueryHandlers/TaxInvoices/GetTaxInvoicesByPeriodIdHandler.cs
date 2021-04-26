using Erc.Households.Api.Queries.TaxInvoices;
using Erc.Households.Domain.Taxes;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers
{
    public class GetTaxInvoiceByPeriodIdHandler : IRequestHandler<GetTaxInvoiceByPeriodId, TaxInvoice>
    {
        private readonly ErcContext _ercContext;

        public GetTaxInvoiceByPeriodIdHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<TaxInvoice> Handle(GetTaxInvoiceByPeriodId request, CancellationToken cancellationToken)
            => await _ercContext.TaxInvoices.FirstOrDefaultAsync(x => x.BranchOfficeId == request.BranchOfficeId && x.PeriodId == request.PeriodId);
    }
}
