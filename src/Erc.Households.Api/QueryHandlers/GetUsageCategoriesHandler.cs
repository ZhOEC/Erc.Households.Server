using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.Queries
{
    public class GetUsageCategoriesHandler : IRequestHandler<GetUsageCategories, IEnumerable<Responses.UsageCategory>>
    {
        private readonly ErcContext _ercContext;
        private readonly IMapper _mapper;

        public GetUsageCategoriesHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Responses.UsageCategory>> Handle(GetUsageCategories request, CancellationToken cancellationToken)
            => await _ercContext.UsageCategories.ProjectTo<Responses.UsageCategory>(_mapper.ConfigurationProvider).OrderBy(x => x.Id).AsNoTracking().ToArrayAsync();
    }
}
