using Erc.Households.Domain.AccountingPoints;
using MediatR;
using System.Collections.Generic;

namespace Erc.Households.Api.Queries
{
    public class GetBuildingTypes : IRequest<IEnumerable<BuildingType>>
    {
    }
}
