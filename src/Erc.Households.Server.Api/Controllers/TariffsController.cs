using System;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Server.Domain.Tariffs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erc.Households.Server.DataAccess.PostgreSql;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TariffsController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public TariffsController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll() 
            => Ok(await _ercContext.Tariffs
                .Include(t => t.Rates)
                .OrderBy(t=>t.Id)
                .ToListAsync());

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
                _ercContext.Entry(existingTariff).CurrentValues.SetValues(tariff);
                await _ercContext.SaveChangesAsync();

                return Ok(existingTariff);
            }
            return NotFound();
        }

        [HttpGet("{id}/rates")]
        public async Task<IActionResult> GetRates(int id)
            => Ok(await _ercContext.Tariffs.Include(t => t.Rates).Where(t => t.Id == id).Select(t => t.Rates).FirstOrDefaultAsync());

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

            var tariff = await _ercContext.Tariffs.Include(t => t.Rates).FirstOrDefaultAsync(t => t.Id == id);
            var rate = tariff?.Rates.FirstOrDefault(tr => tr.Id == rateId);

            if (rate is null)
                return NotFound();

            _ercContext.Entry(rate).CurrentValues.SetValues(tariffRate);

            await _ercContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}/rates/{rateId}")]
        public async Task<IActionResult> DeleteRate(int id, int rateId)
        {
            var tariff = await _ercContext.Tariffs.Include(t => t.Rates).FirstOrDefaultAsync(t => t.Id == id);
            var rate = tariff?.Rates.FirstOrDefault(tr => tr.Id == rateId);

            if (rate is null)
                return NotFound();

            tariff.RemoveRate(rate);

            await _ercContext.SaveChangesAsync();

            return Ok();
        }
    }
}