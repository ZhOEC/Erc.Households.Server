using System.Threading.Tasks;
using Erc.Households.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExemptionCategoriesController : ControllerBase
    {
        IMediator _mediator;

        public ExemptionCategoriesController(IMediator mediator) => _mediator = mediator;

        [HttpGet("")]
        public async Task<IActionResult> GetListAsync() => Ok(await _mediator.Send(new GetExemptionCategories()));
    }
}
