using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Api.Queries.Invoices;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ErcControllerBase
    {
        readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetList(int accountingPointId, int pageNumber, int pageSize)
        {
            var invoices = await _mediator.Send(new GetInvoicesByAccountingPoint(accountingPointId, pageNumber, pageSize));
            Response.Headers.Add("X-Total-Count", invoices.TotalItemCount.ToString());

            return Ok(invoices);
        }
    }
}
