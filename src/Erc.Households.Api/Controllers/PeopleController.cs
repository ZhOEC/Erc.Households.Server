using Erc.Households.Api.Queries;
using Erc.Households.Commands;
using Erc.Households.Domain;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ErcContext _ercContext;
        private readonly IMediator _mediator;

        public PeopleController(ErcContext ercContext, IMediator mediator)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> SearchPeople(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return Ok(await _ercContext.People.Where(x => x.TaxCode.Contains(searchString) || x.IdCardNumber.Contains(searchString)).Take(10).ToArrayAsync());
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id) => Ok(await _mediator.Send(new GetPersonById(id)));

        [HttpPut("{id}")]
        public async Task<Unit> Update(Person person)
        {
            return await _mediator.Send(new UpdatePersonCommand(person.Id, person.IdCardNumber, person.IdCardIssuanceDate, person.IdCardIssuer, person.IdCardExpDate,
                                                                person.MobilePhones, person.Email));
        }
    }
}