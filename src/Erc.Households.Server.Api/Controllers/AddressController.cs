using Erc.Households.Server.DataAccess.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public AddressController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet("cities")]
        public async Task<IActionResult> Cities(int branchOfficeId)
        {
            var boIds = _ercContext.BranchOffices.Where(x => x.Id == branchOfficeId).Select(g => g.DistrictIds);
            return Ok(await _ercContext.Cities.Where(x => boIds.Any(t => t.Contains(x.DistrictId))).ToListAsync());
        }

        [HttpGet("streets")]
        public async Task<IActionResult> Streets(int cityId)
        {
            return Ok(await _ercContext.Streets.Where(x => x.CityId == cityId).ToListAsync());
        }
    }
}