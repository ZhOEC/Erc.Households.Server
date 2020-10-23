using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Api.Queries;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace Erc.Households.Api.QueryHandlers
{
    public class GetTaxInvoicesByPartHandler : IRequestHandler<GetTaxInvoicesByPart, IPagedList<Responses.TaxInvoice>>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetTaxInvoicesByPartHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<IPagedList<Responses.TaxInvoice>> Handle(GetTaxInvoicesByPart request, CancellationToken cancellationToken)
            => await _ercContext.TaxInvoices
                        .Include(i => i.BranchOffice)
                        .Where(p => p.BranchOfficeId == request.BranchOfficeId)
                        .ProjectTo<Responses.TaxInvoice>(_mapper.ConfigurationProvider)
                        .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
