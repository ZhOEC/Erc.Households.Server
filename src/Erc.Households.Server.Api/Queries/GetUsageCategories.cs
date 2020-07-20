using System.Collections.Generic;
using MediatR;

namespace Erc.Households.Api.Queries
{
    public class GetUsageCategories : IRequest<IEnumerable<Responses.UsageCategory>> 
    {
    }
}
