using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Api.Queries.Payments;
using Erc.Households.Api.Responses;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.AccountingPoints
{
    public class GetPaymentsBatchByIdHandler : IRequestHandler<GetPaymentsBatchById, PaymentsBatch>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetPaymentsBatchByIdHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<PaymentsBatch> Handle(GetPaymentsBatchById request, CancellationToken cancellationToken)
        {
            return await _ercContext.PaymentBatches
                .ProjectTo<PaymentsBatch>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
        }
    }
}
