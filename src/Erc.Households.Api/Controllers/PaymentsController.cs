using System.Threading.Tasks;
using Erc.Households.Api.Authorization;
using Erc.Households.Api.Queries.Payments;
using Erc.Households.Api.Requests;
using Erc.Households.Commands.Payments;
using Erc.Households.DataAccess.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ApplicationRoles.Operator)]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentsController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            return Ok(await _mediator.Send(new GetPaymentById(id)));
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Post(NewPayment newPayment)
        {
            await _mediator.Send(new CreatePaymentCommand(newPayment.AccountingPointId, newPayment.PayDate, newPayment.Amount, 
                                                          newPayment.PayerInfo, newPayment.Type, newPayment.BatchId));
            await _unitOfWork.SaveWorkAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdatedPayment updatedPayment)
        {
            await _mediator.Send(new UpdatePaymentCommand(updatedPayment.Id, updatedPayment.BatchId, updatedPayment.AccountingPointId, updatedPayment.PayDate,
                                                          updatedPayment.Amount, updatedPayment.PayerInfo, updatedPayment.Type));
            await _unitOfWork.SaveWorkAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeletePaymentCommand(id));
            await _unitOfWork.SaveWorkAsync();

            return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(int accountingPointId, int pageNumber, int pageSize)
        {
            var pagedList = await _mediator.Send(new GetPaymentsByAccountingPoint(accountingPointId, pageNumber, pageSize));
            Response.Headers.Add("X-Total-Count", pagedList.TotalItemCount.ToString());

            return Ok(pagedList);
        }
    }
}
