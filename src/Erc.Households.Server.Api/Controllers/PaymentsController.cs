using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Api.Queries.Payments;
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
            return Ok(pagedList.ToList());
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
