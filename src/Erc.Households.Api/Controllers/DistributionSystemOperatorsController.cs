using System;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Domain.Shared;
using Erc.Households.EF.PostgreSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Erc.Households.Api.Controllers
{
    [Route("api/distribution-system-operators")]
    [ApiController]
    public class DistributionSystemOperatorsController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public DistributionSystemOperatorsController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(Commodity commodity = Commodity.ElectricPower)
            => Ok(await _ercContext.DistributionSystemOperators.Where(d => d.Commodity == commodity).ToArrayAsync());
    }
}