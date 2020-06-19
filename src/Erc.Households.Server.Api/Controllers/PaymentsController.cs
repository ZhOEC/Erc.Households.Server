using System.Threading.Tasks;
using Erc.Households.Api.Queries.Payments;
using Erc.Households.Api.Requests;
using Erc.Households.DataAccess.Core;
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
        private readonly IUnitOfWork _unitOfWork;

        public PaymentsController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
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
        public async Task<IActionResult> Post(NewPayment newPayment)
        {
            var payment = await _mediator.Send(new AddPayment(newPayment));

            if (payment is null)
                return BadRequest();

            await _unitOfWork.SaveWorkAsync();

            return Ok(payment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _mediator.Send(new GetPaymentById(id));

            if (payment is null)
                return NotFound();
            else if (payment.Status == PaymentStatus.Processed)
                return BadRequest("Платіж проведений, і поки не може бути видалений");

            if (await _mediator.Send(new DeletePayment(payment.Id)))
                await _unitOfWork.SaveWorkAsync();

            return Ok();
        }
    }
}
