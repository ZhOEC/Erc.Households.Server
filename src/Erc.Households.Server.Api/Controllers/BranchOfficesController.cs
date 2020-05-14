using System;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Server.Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erc.Households.EF.PostgreSQL;

namespace Erc.Households.Backend.Api.Controllers
{
    [Route("api/branch-offices")]
    [ApiController]
    [Authorize(Roles = ApplicationRoles.Operator)]
    public class BranchOfficesController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public BranchOfficesController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var groups = User.Claims.Where(c => string.Equals(c.Type, "Group", StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value);
            return Ok(await _ercContext.BranchOffices.Where(bo => groups.Contains(bo.StringId)).ToListAsync());
        }
    }
}