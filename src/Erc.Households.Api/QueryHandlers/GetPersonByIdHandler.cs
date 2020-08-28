using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Api.Queries;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers
{
    public class GetPersonByIdHandler : IRequestHandler<GetPersonById, Domain.Person>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetPersonByIdHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<Domain.Person> Handle(GetPersonById request, CancellationToken cancellationToken)
        {
            return await _ercContext.People
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);
        }
    }
}
