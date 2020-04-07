using Erc.Households.Server.DataAccess.PostgreSql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Server.Api.Controllers
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
            if (!string.IsNullOrEmpty(searchString) && searchString.Length > 0)
            {
                return Ok(await _ercContext.People.Where(x => EF.Functions.ILike(x.TaxCode, $"{searchString}%") || EF.Functions.ILike(x.IdCardNumber, $"{searchString}")).Take(10).ToListAsync());
            } else
            {
                return BadRequest();
            }
        }
    }
}