using System;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.DataAccess.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Erc.Households.WebApi.Controllers
{
    [Route("api/branch-offices")]
    [ApiController]
    [Authorize]
    public class BranchOfficesController : ErcControllerBase
    {
        private readonly ErcContext _ercContext;

        public BranchOfficesController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(
                await _ercContext.BranchOffices
                .Include(b => b.CurrentPeriod)
                .Where(bo => UserGroups.Contains(bo.StringId))
                .ToArrayAsync()
                );
        }
    }
}