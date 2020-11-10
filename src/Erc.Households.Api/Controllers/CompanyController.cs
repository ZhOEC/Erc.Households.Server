using Erc.Households.Api.Queries;
using Erc.Households.Commands;
using Erc.Households.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComapny(int id) => Ok(await _mediator.Send(new GetCompanyById(id)));

        [HttpPut("{id}")]
        public async Task<Unit> Update(Company company)
        {
            return await _mediator.Send(new UpdateCompanyCommand(company.Id, company.Name, company.ShortName, company.DirectorName, company.Address, company.Email, company.Www, company.TaxpayerPhone, company.StateRegistryCode,
                company.TaxpayerNumber, company.BookkeeperName, company.BookkeeperTaxNumber));
        }
    }
}