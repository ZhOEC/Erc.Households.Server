using System;
using System.Net.Http;
using System.Threading.Tasks;
using Erc.Households.Domain.Shared;
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

        [HttpGet("{type}/{id:int}")]
        public async Task<IActionResult> GetById(Commodity type, int id)
        {
            var client = _clientFactory.CreateClient("print-bills");
            return File(await client.GetStreamAsync($"{type}/{id}"),
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{id}.xlsx");
        }

        [HttpGet("")]
        public async Task<IActionResult> GetBillsByPeriod([FromQuery(Name = "file_type")] FileType fileType, [FromQuery(Name = "branch_office_id")] int branchOfficeId,
            Commodity commodity, [FromQuery(Name = "period_id")] int periodId)
        {
            var client = _clientFactory.CreateClient("print-bills");
            client.Timeout = TimeSpan.FromMinutes(30); // HACK

            return File(await client.GetStreamAsync($"?file_type={fileType}&branch_office_id={branchOfficeId}&commodity={commodity}&period_id={periodId}"),
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{branchOfficeId}_{periodId}.xlsx");
        }
    }
}
