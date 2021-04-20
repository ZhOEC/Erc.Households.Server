using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Api.Queries.TaxInvoices;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.TaxInvoices
{
    public class GetTaxInvoiceByIdHandler : IRequestHandler<GetTaxInvoiceById, Requests.ExportTaxInvoice>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetTaxInvoiceByIdHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<Requests.ExportTaxInvoice> Handle(GetTaxInvoiceById request, CancellationToken cancellationToken)
        {
            return await _ercContext.TaxInvoices
                .Include(t => t.BranchOffice)
                    .ThenInclude(t => t.Company)
                .ProjectTo<Requests.ExportTaxInvoice>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
