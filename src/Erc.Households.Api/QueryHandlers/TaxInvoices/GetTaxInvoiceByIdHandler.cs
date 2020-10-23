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
    public class GetTaxInvoiceByIdHandler : IRequestHandler<GetTaxIonviceById, Requests.DownloadTaxInvoice>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetTaxInvoiceByIdHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<Requests.DownloadTaxInvoice> Handle(GetTaxIonviceById request, CancellationToken cancellationToken)
        {
            return await _ercContext.TaxInvoices
                .Include(t => t.BranchOffice)
                    .ThenInclude(t => t.Company)
                .ProjectTo<Requests.DownloadTaxInvoice>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
