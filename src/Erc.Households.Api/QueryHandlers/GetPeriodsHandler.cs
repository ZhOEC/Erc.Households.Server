using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Erc.Households.Domain.Billing;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Api.Queries;

namespace Erc.Households.Api.QueryHandlers
{
    public class GetPeriodsHandler : IRequestHandler<GetPeriods, IEnumerable<Period>>
    {
        private readonly ErcContext _ercContext;

        public GetPeriodsHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<IEnumerable<Period>> Handle(GetPeriods request, CancellationToken cancellationToken)
            => await _ercContext.Periods.AsNoTracking().ToListAsync();
    }
}
