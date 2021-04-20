using MediatR;
using System.Collections.Generic;
using Erc.Households.Domain.Billing;

namespace Erc.Households.Api.Queries
{
    public class GetPeriods : IRequest<IEnumerable<Period>>
    {
    }
}
