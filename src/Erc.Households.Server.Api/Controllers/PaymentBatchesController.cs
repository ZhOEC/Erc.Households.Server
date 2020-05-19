using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Domain.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Api.Requests;
using Erc.Households.Api.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentBatchesController : ControllerBase
    {
        private readonly ErcContext _ercContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PaymentBatchesController(ErcContext ercContext, IWebHostEnvironment hostingEnvironment)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetPart(int pageNumber, int pageSize, bool showClosed)
        {
            var totalCount = await _ercContext.PaymentBatches.CountAsync(x => showClosed ? x.IsClosed || !x.IsClosed : !x.IsClosed);
 
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            return Ok(
                await _ercContext.PaymentBatches
                    .Where(x => showClosed ? x.IsClosed || !x.IsClosed : !x.IsClosed)
                    .OrderByDescending(x => x.Id)
                    .Skip((pageNumber - 1) * pageSize > totalCount ? 0 : (pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
                );
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] NewPaymentBatch paymentBatch)
        {
            var paymentChannel = _ercContext.PaymentChannels.Where(x => x.Id == paymentBatch.PaymentChannelId).FirstOrDefault();

            if (paymentChannel is null)
                return NotFound("Payment channel not found!");

            var paymentList = new List<Payment>();
            if (paymentBatch.UploadFile != null)
            {
                var extFile = Path.GetExtension(paymentBatch.UploadFile.FileName);
                if (extFile == ".dbf") paymentList = new PaymentsDbfParser(_hostingEnvironment, _ercContext).Parser(paymentBatch.UploadFile, paymentChannel);
                else if (extFile == ".xls" || extFile == ".xlsx") return BadRequest("Excel files not yet supported");
            }

            var payBatch = new PaymentBatch(
                paymentBatch.PaymentChannelId,
                paymentList
            );

            _ercContext.PaymentBatches.Add(payBatch);
            await _ercContext.SaveChangesAsync();

            return Ok(payBatch);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var paymentBatch = await _ercContext.PaymentBatches.FirstOrDefaultAsync(x => x.Id == id);

            if (paymentBatch is null)
                return NotFound();
            else if (paymentBatch.Payments.Any(x => x.Status == PaymentStatus.Processed))
                return BadRequest("Пачка містить рознесені платежі");

            _ercContext.Remove(paymentBatch);
            await _ercContext.SaveChangesAsync();

            return Ok();
        }
    }
}

