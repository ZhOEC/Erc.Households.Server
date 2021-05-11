using System.Threading.Tasks;
using Erc.Households.Api.Queries;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Domain;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkersController : ErcControllerBase
    {
        private readonly ErcContext _ercContext;
        readonly IMediator _mediator;

        public MarkersController(ErcContext ercContext, IMediator mediator)
        {
            _ercContext = ercContext;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPart(int pageNumber, int pageSize)
        {
            var markers = await _mediator.Send(new GetMarkersByPart(pageNumber, pageSize));
            Response.Headers.Add("X-Total-Count", markers.TotalItemCount.ToString());

            return Ok(markers);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Marker marker)
        {
            await _ercContext.Markers.AddAsync(marker);
            await _ercContext.SaveChangesAsync();

            return Ok(marker);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Marker marker)
        {
            var existingMarker = await _ercContext.Markers.FindAsync(marker.Id);

            if (existingMarker == null)
                return NotFound();

            _ercContext.Entry(existingMarker).CurrentValues.SetValues(marker);
            await _ercContext.SaveChangesAsync();

            return Ok(existingMarker);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingMarker = await _ercContext.Markers.FindAsync(id);

            if (existingMarker == null)
                return NotFound();

            _ercContext.Remove(existingMarker);
            await _ercContext.SaveChangesAsync();

            return Ok();
        }
    }
}
