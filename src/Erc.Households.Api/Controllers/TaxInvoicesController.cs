using System.Threading.Tasks;
using Erc.Households.Api.Helpers;
using Erc.Households.Api.Queries;
using Erc.Households.Api.Queries.TaxInvoices;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Net.Mime;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxInvoicesController : ErcControllerBase
    {
        readonly IMediator _mediator;
        
        public TaxInvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPart(int branchOfficeId, int pageNumber, int pageSize)
        {
            var taxInvoices = await _mediator.Send(new GetTaxInvoicesByPart(branchOfficeId, pageNumber, pageSize));
            Response.Headers.Add("X-Total-Count", taxInvoices.Count.ToString());

            return Ok(taxInvoices);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> GetTaxInvoiceById(int id)
        {
            var xmlData = await _mediator.Send(new GetTaxIonviceById(id));
            var ms = new TaxInvoiceXmlExporter().Export(xmlData);
            ms.Position = 0;

            return File(ms, "application/xml", $"{id}.xml");
        }
    }
}
