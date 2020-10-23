using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Api.Queries.Invoices;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace Erc.Households.Api.QueryHandlers.AccountingPoints
{
    public class GetInvoicesByAccountingPointHandler : IRequestHandler<GetInvoicesByAccountingPoint, IPagedList<Responses.Invoice>>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetInvoicesByAccountingPointHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<IPagedList<Responses.Invoice>> Handle(GetInvoicesByAccountingPoint request, CancellationToken cancellationToken)
            => await _ercContext.Invoices
                        .Include(i => i.Period)
                        .Include(i => i.InvoicePaymentItems)
                        .Where(a => a.AccountingPointId == request.AccountingPointId)
                        .OrderByDescending(i => i.PeriodId)
                        .ThenByDescending(i => i.FromDate)
                        .ThenByDescending(i => i.Id)
                        .ProjectTo<Responses.Invoice>(_mapper.ConfigurationProvider)
                        .ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
