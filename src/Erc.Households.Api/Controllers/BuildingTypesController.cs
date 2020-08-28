using System.Threading.Tasks;
using Erc.Households.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingTypesController : ControllerBase
    {
        IMediator _mediator;

        public BuildingTypesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetBuildingTypes()));
    }
}
