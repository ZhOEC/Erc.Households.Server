using System.Threading.Tasks;
using Erc.Households.Api.Queries.Payments;
using Erc.Households.Api.Requests;
using Erc.Households.Domain.Payments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPart(int paymentsBatchId, int pageNumber, int pageSize, bool showProcessed)
        {
            var pagedList = await _mediator.Send(new GetPaymentsByPart(paymentsBatchId, pageNumber, pageSize, showProcessed));
 
            Response.Headers.Add("X-Total-Count", pagedList.TotalItemCount.ToString());
            return Ok(pagedList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            return Ok(await _mediator.Send(new GetPaymentById(id)));
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Post(NewPayment newPayment)
        {
            return await _mediator.Send(new CreatePaymentCommand(newPayment));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Update(UpdatedPayment updatedPayment)
        {
            var payment = await _mediator.Send(new GetPaymentById(updatedPayment.Id));
            
            if (payment is null)
                return NotFound();
            else if (payment.Status == PaymentStatus.Processed)
                return BadRequest("Платіж проведений, і не може редагуватися");

            return await _mediator.Send(new UpdatePaymentCommand(updatedPayment));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            var payment = await _mediator.Send(new GetPaymentById(id));

            if (payment is null)
                return NotFound();
            else if (payment.Status == PaymentStatus.Processed)
                return BadRequest("Платіж проведений, і не може бути видалений");

            return await _mediator.Send(new DeletePaymentCommand(payment.Id));
        }
    }
}
