using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Domain.Shared;
using Erc.Households.Domain.Shared.Tariffs;
using Microsoft.AspNetCore.Authorization;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "User")]
    public class TariffsController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public TariffsController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(Commodity commodity = Commodity.ElectricPower)
            => await _ercContext.Tariffs.Where(t => t.Commodity == commodity)
                .OrderBy(t => t.Id)
                .ToArrayAsync().ContinueWith(t => Ok(t.Result));

        [HttpPost("")]
        public async Task<IActionResult> AddNew(Tariff tariff)
        {
            _ercContext.Tariffs.Add(tariff);
            await _ercContext.SaveChangesAsync();
            
            return Ok(tariff);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Tariff tariff)
        {
            if (tariff.Id != id || id == 0)
                return BadRequest();

            var existingTariff = await _ercContext.Tariffs.FindAsync(id);
            if (existingTariff != null)
            {
                existingTariff.Name = tariff.Name;
                await _ercContext.SaveChangesAsync();

                return Ok(existingTariff);
            }
            return NotFound();
        }

        [HttpPost("{id}/rates")]
        public async Task<IActionResult> AddRate(int id, TariffRate tariffRate)
        {
            var tariff = await _ercContext.Tariffs.FindAsync(id);
            
            tariff.AddRate(tariffRate);
            await _ercContext.SaveChangesAsync();
            
            return Ok(tariffRate);
        }

        [HttpPut("{id}/rates/{rateId}")]
        public async Task<IActionResult> UpdateRate(int id, int rateId, TariffRate tariffRate)
        {
            if (rateId != tariffRate.Id || id == 0 || rateId == 0)
                return BadRequest();

            var tariff = await _ercContext.Tariffs.FirstOrDefaultAsync(t => t.Id == id);
            

            if (tariff is null)
                return NotFound();

            tariff.UpdateRate(tariffRate);

            await _ercContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}/rates/{rateId}")]
        public async Task<IActionResult> DeleteRate(int id, int rateId)
        {
            if (id == 0)
                return BadRequest();

            var tariff = await _ercContext.Tariffs.FirstOrDefaultAsync(t => t.Id == id);

            if (tariff is null)
                return NotFound();

            tariff.RemoveRate(rateId);
            await _ercContext.SaveChangesAsync();

            return Ok();
        }
    }
}