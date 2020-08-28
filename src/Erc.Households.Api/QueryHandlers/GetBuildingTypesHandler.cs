using Erc.Households.Api.Queries;
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers
{
    public class GetBuildingTypesHandler : IRequestHandler<GetBuildingTypes, IEnumerable<BuildingType>>
    {
        private readonly ErcContext _ercContext;

        public GetBuildingTypesHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<IEnumerable<BuildingType>> Handle(GetBuildingTypes request, CancellationToken cancellationToken)
            => await _ercContext.BuildingTypes.AsNoTracking().ToArrayAsync();
    }
}
