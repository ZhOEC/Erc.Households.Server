using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Erc.Households.WebApi.Helpers
{
    public class ClaimTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var ci = (ClaimsIdentity)principal.Identity;

            foreach (var c in principal.Claims.Where(c => c.Properties.Any(p => p.Value == "group")).ToList())
                ci.AddClaim(new Claim("Group", c.Value.Substring(1).ToLower()));

            return Task.FromResult(principal);
        }
    }
}
