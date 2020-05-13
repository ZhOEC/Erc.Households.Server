using Erc.Households.DataAccess.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.WebApi.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public PeopleController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet("")]
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
    }
}