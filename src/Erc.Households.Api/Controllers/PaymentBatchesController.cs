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
using MediatR;
using Erc.Households.Api.Queries.Payments;
using Erc.Households.BranchOfficeManagment.Core;
using Microsoft.AspNetCore.Authorization;
using Erc.Households.Api.Controllers;
using Erc.Households.Commands.PaymentBatches;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentBatchesController : ErcControllerBase
    {
        private readonly ErcContext _ercContext;
        private readonly IMediator _mediator;
        private readonly IBranchOfficeService _branchOfficeService;

        public PaymentBatchesController(ErcContext ercContext, IMediator mediator, IBranchOfficeService branchOfficeService)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
            _mediator = mediator;
            _branchOfficeService = branchOfficeService;
        }

        [HttpGet("{id:int}/payments")]
        public async Task<IActionResult> GetPayments(int id, int pageNumber, int pageSize, bool showProcessed)
        {
            var pagedList = await _mediator.Send(new GetPaymentsByPart(id, pageNumber, pageSize, showProcessed));

            Response.Headers.Add("X-Total-Count", pagedList.TotalItemCount.ToString());
            return Ok(pagedList);
        }

        [HttpGet]
        public async Task<IActionResult> GetPart(int pageNumber, int pageSize, bool showClosed)
        {
            var pagedList = await _mediator.Send(new GetPaymentBatchesByPart(UserGroups, pageNumber, pageSize, showClosed));

            Response.Headers.Add("X-Total-Count", pagedList.TotalItemCount.ToString());
            return Ok(pagedList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            return Ok(await _mediator.Send(new GetPaymentsBatchById(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] NewPaymentsBatch paymentBatch)
        {
            var paymentChannel = await _mediator.Send(new GetPaymentChannelById(paymentBatch.PaymentChannelId));

            if (paymentChannel is null)
                return BadRequest("Payment channel not found!");

            var paymentList = new List<Domain.Payments.Payment>();
            if (paymentBatch.UploadFile != null)
            {
                var extFile = Path.GetExtension(paymentBatch.UploadFile.FileName);
                if (extFile == ".dbf") paymentList = new PaymentsDbfParser(_ercContext, _branchOfficeService).Parser(paymentBatch.BranchOfficeId, paymentChannel, paymentBatch.UploadFile);
                else if (extFile == ".xls" || extFile == ".xlsx") return BadRequest("Excel files not yet supported");
            }

            var payBatch = new PaymentsBatch(
                paymentBatch.BranchOfficeId,
                paymentBatch.PaymentChannelId,
                paymentBatch.IncomingDate,
                paymentList
            );

            _ercContext.PaymentBatches.Add(payBatch);
            await _ercContext.SaveChangesAsync();

            return await GetOne(payBatch.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(PaymentsBatch paymentsBatch)
        {
            var pc = await _ercContext.PaymentBatches.FindAsync(paymentsBatch.Id);

            if (pc is null)
                return NotFound();

            _ercContext.Entry(pc).CurrentValues.SetValues(paymentsBatch);
            await _ercContext.SaveChangesAsync();

            return Ok();
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

        [HttpPost("{id}/processing")]
        public async Task<IActionResult> Process(int id)
        {
            await _mediator.Send(new ProcessPaymentBatch(id));
            
            await _ercContext.SaveChangesAsync();

            return Ok();
        }
    }
}

