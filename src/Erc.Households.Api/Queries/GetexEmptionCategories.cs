using System.Collections.Generic;
using MediatR;


namespace Erc.Households.Api.Queries
{
    public class GetExemptionCategories : IRequest<IEnumerable<Domain.Shared.Exemptions.ExemptionCategory>> 
    {
    }
}
