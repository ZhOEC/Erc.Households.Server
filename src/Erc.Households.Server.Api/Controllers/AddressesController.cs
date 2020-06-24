using Erc.Households.EF.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public AddressesController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet("cities")]
        public async Task<IActionResult> Cities(int branchOfficeId)
        {
            var boIds = await _ercContext.BranchOffices.Where(x => x.Id == branchOfficeId).Select(g => g.DistrictIds).FirstAsync();
            return Ok(_ercContext.Cities
                .Include(c => c.District)
                .Where(x => boIds.Contains(x.DistrictId))
                .Select(c => new { c.Id, c.Name, DistrictName = c.District.Name, c.IsRegionCity, c.IsDistrictCity })
                .ToArray()
                .OrderBy(c => c.IsDistrictCity ? 1 : 2)
                .ThenBy(c => c.Name.Substring(c.Name.IndexOf(" ")))
                .ThenBy(c => c.DistrictName)
                );
        }

        [HttpGet("streets")]
        public async Task<IActionResult> Streets(int cityId)
        {
            return Ok(await _ercContext.Streets.Where(x => x.CityId == cityId)
                .OrderBy(c => c.Name.Substring(c.Name.IndexOf(" ")))
                .ThenBy(c => c.Name)
                .ToArrayAsync());
        }
    }
}