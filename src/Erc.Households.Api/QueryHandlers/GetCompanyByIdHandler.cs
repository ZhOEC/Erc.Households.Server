using AutoMapper;
using Erc.Households.Api.Queries;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers
{
    public class GetComapnyByIdHandler : IRequestHandler<GetCompanyById, Domain.Company>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetComapnyByIdHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<Domain.Company> Handle(GetCompanyById request, CancellationToken cancellationToken)
        {
            return await _ercContext.Company
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
        }
    }
}
