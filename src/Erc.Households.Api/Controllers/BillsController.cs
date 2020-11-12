using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ErcControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public BillsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var client = _clientFactory.CreateClient("print-bills");
            
            return File( await client.GetStreamAsync(id.ToString()), contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{id}.xlsx");
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery(Name="branch_office_id")]int branchOfficeId, [FromQuery(Name="period_id")] int periodId)
        {
            var client = _clientFactory.CreateClient("print-bills");

            return File(await client.GetStreamAsync($"?period_id={periodId}&branch_office_id={branchOfficeId}"), contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{branchOfficeId}_{periodId}.xlsx");
        }
    }
}
