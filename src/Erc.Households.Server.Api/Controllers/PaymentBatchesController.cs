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
using MediatR;
using Erc.Households.Api.Queries.AccountingPoints;
using Erc.Households.Api.Queries.Payments;
using Erc.Households.BranchOfficeManagment.Core;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentBatchesController : ControllerBase
    {
        private readonly ErcContext _ercContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMediator _mediatR;
        IBranchOfficeService _branchOfficeService;

        public PaymentBatchesController(ErcContext ercContext, IWebHostEnvironment hostingEnvironment, IMediator mediator, IBranchOfficeService branchOfficeService)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
            _hostingEnvironment = hostingEnvironment;
            _mediatR = mediator;
            _branchOfficeService = branchOfficeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPart(int pageNumber, int pageSize, bool showClosed)
        {
            var pagedList = await _mediatR.Send(new GetPaymentBatchesByPart(pageNumber, pageSize, showClosed));

            Response.Headers.Add("X-Total-Count", pagedList.TotalItemCount.ToString());
            return Ok(pagedList.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] NewPaymentsBatch paymentBatch)
        {
            var paymentChannel = await _mediatR.Send(new GetPaymentChannelById(paymentBatch.PaymentChannelId));

            if (paymentChannel is null)
                return BadRequest("Payment channel not found!");

            var paymentList = new List<Payment>();
            if (paymentBatch.UploadFile != null)
            {
                var extFile = Path.GetExtension(paymentBatch.UploadFile.FileName);
                if (extFile == ".dbf") paymentList = new PaymentsDbfParser(_hostingEnvironment, _ercContext, _branchOfficeService).Parser(paymentBatch.UploadFile, paymentChannel, paymentBatch.BranchOfficeId);
                else if (extFile == ".xls" || extFile == ".xlsx") return BadRequest("Excel files not yet supported");
            }

            var payBatch = new PaymentsBatch(
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

