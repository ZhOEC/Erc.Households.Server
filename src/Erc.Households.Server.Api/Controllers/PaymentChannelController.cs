using System;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Server.DataAccess.EF;
using Erc.Households.Server.Domain.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentChannelController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public PaymentChannelController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _ercContext.PaymentChannels.OrderBy(x => x.Id).ToListAsync());

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> Add(PaymentChannel paymentChannel)
        {
            _ercContext.PaymentChannels.Add(paymentChannel);
            await _ercContext.SaveChangesAsync();

            return Ok(paymentChannel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(PaymentChannel paymentChannel)
        {
            var pc = await _ercContext.PaymentChannels.Where(x => x.Id == paymentChannel.Id).FirstOrDefaultAsync();

            if (pc is null)
                return NotFound();

            _ercContext.Entry(pc).CurrentValues.SetValues(paymentChannel);
            await _ercContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var paymentChannel = await _ercContext.PaymentChannels.FirstOrDefaultAsync(x => x.Id == id);
            _ercContext.Remove(paymentChannel);
            await _ercContext.SaveChangesAsync();

            return Ok();
        }
    }
}
