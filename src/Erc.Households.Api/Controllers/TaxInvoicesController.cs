using System.Threading.Tasks;
using Erc.Households.Api.Helpers;
using Erc.Households.Api.Queries;
using Erc.Households.Api.Queries.TaxInvoices;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Erc.Households.Domain.Taxes;
using Erc.Households.EF.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxInvoicesController : ErcControllerBase
    {
        private readonly ErcContext _ercContext;
        readonly IMediator _mediator;
        
        public TaxInvoicesController(ErcContext ercContext, IMediator mediator)
        {
            _ercContext = ercContext;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPart(int branchOfficeId, int pageNumber, int pageSize)
        {
            var taxInvoices = await _mediator.Send(new GetTaxInvoicesByPart(branchOfficeId, pageNumber, pageSize));
            Response.Headers.Add("X-Total-Count", taxInvoices.Count.ToString());

            return Ok(taxInvoices);
        }

        [HttpGet("{id}/export")]
        public async Task<IActionResult> GetTaxInvoiceById(int id)
        {
            var xmlData = await _mediator.Send(new GetTaxInvoiceById(id));
            var ms = new TaxInvoiceXmlExporter().Export(xmlData);
            ms.Position = 0;

            return File(ms, "application/xml", $"{id}.xml");
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(TaxInvoice taxInvoice)
        {
            await _ercContext.TaxInvoices.AddAsync(taxInvoice);
            await _ercContext.SaveChangesAsync();
            
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var taxInvoice = await _ercContext.TaxInvoices.FirstOrDefaultAsync(x => x.Id == id);

            if (taxInvoice is null)
                return NotFound();
            else if ((DateTime.Now - taxInvoice.CreationDate).TotalDays > 7)
                return BadRequest("Податкова накладна занадто стара для цього");

            _ercContext.Remove(taxInvoice);
            await _ercContext.SaveChangesAsync();

            return Ok();
        }
    }
}
