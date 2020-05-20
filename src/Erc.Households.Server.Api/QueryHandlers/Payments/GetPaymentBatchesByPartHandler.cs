using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Api.Queries.AccountingPoints;
using Erc.Households.Api.Responses;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace Erc.Households.Api.QueryHandlers.AccountingPoints
{
    public class GetPaymentBatchesByPartHandler : IRequestHandler<GetPaymentBatchesByPart, IPagedList<PaymentsBatch>>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetPaymentBatchesByPartHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<IPagedList<PaymentsBatch>> Handle(GetPaymentBatchesByPart request, CancellationToken cancellationToken)
        {
            return await _ercContext.PaymentBatches
                .Include(t => t.PaymentChannel)
                .Where(x => request.ShowClosed || !x.IsClosed)
                .ProjectTo<PaymentsBatch>(_mapper.ConfigurationProvider)
                .OrderByDescending(x => x.Id)
                .ToPagedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
