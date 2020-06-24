using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Api.Queries.Payments;
using Erc.Households.Api.Responses;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.AccountingPoints
{
    public class GetPaymentByIdHandler : IRequestHandler<GetPaymentById, Payment>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetPaymentByIdHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<Payment> Handle(GetPaymentById request, CancellationToken cancellationToken)
        {
            return await _ercContext.Payments
                .ProjectTo<Payment>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
        }
    }
}
