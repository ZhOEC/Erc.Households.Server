using Erc.Households.Api.Helpers;
using Erc.Households.BranchOfficeManagment.Core;
using Erc.Households.Commands;
using Erc.Households.ConsumptionParser.Core;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.UsageParser.Core;
using Erc.Households.UsageParser.Xlsx.NaturalGas;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConsumptionsController : ErcControllerBase
    {
        private readonly ErcContext _ercContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBranchOfficeService _branchOfficeService;
        private readonly IBus _bus;

        public ConsumptionsController(ErcContext ercContext, IServiceProvider serviceProvider, IBranchOfficeService branchOfficeService, IBus bus)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
            _serviceProvider = serviceProvider;
            _branchOfficeService = branchOfficeService;
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFilesAsync([FromForm] IEnumerable<IFormFile> files)
        {
            if (files.Count() > 0)
            {
                var errors = new[] { new { FileName = "", Errors = Array.Empty<ParsedConsumption>().AsEnumerable() } }.ToList();
                errors.Clear();
                IConsumptionParser parser = _serviceProvider.GetRequiredService<XlsxNaturalGasConsumptionParser>();

                foreach (var file in files)
                {
                    var consumptions = parser.Parse(file.OpenReadStream());
                    if (consumptions.Any(c => !c.IsParsed))
                        errors.Add(new
                        {
                            file.FileName,
                            Errors = consumptions.Where(c => !c.IsParsed)
                        });
                    else
                        await Task.WhenAll(consumptions.Select(c => _bus.Publish<CalculateAccountingPoint>(c)).ToList());
                }
                return Ok(errors);
            }

            return BadRequest();
        }
    }
}
