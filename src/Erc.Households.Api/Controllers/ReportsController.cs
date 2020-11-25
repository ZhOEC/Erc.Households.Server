using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ErcControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public ReportsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("{**slug}")]
        public async Task<IActionResult> Get(string slug, [FromQuery(Name="branch_office_id")]int branchOfficeId, [FromQuery(Name="period_id")] int periodId)
        {
            var client = _clientFactory.CreateClient("reports");
            var report = await client.GetAsync($"{slug}?period_id={periodId}&branch_office_id={branchOfficeId}");
            report.EnsureSuccessStatusCode();
            return File(await report.Content.ReadAsStreamAsync(), contentType: report.Content.Headers.ContentType.MediaType, report.Content.Headers.ContentDisposition.FileName);
        }
    }
}
