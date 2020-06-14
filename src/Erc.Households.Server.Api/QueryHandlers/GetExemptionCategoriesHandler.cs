using Erc.Households.Domain.Exemptions;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.Queries
{
    public class GetExemptionCategoriesHandler : IRequestHandler<GetExemptionCategories, IEnumerable<ExemptionCategory>>
    {
        private readonly ErcContext _ercContext;

        public GetExemptionCategoriesHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<IEnumerable<ExemptionCategory>> Handle(GetExemptionCategories request, CancellationToken cancellationToken)
            => await _ercContext.ExemptionCategories.AsNoTracking().ToArrayAsync();
    }
}
