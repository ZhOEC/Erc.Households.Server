using Erc.Households.Api.Helpers;
using Erc.Households.BranchOfficeManagment.Core;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConsumptionController : ErcControllerBase
    {
        private readonly ErcContext _ercContext;
        private readonly IMediator _mediatR;
        private readonly IBranchOfficeService _branchOfficeService;

        public ConsumptionController(ErcContext ercContext, IMediator mediator, IBranchOfficeService branchOfficeService)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
            _mediatR = mediator;
            _branchOfficeService = branchOfficeService;
        }

        [HttpPost]
        public IActionResult Add([FromForm] List<IFormFile> files)
        {
            var list = new List<DataFile>();
            if (files.Count > 0)
            {
                files.ForEach(file =>
                {
                    var extFile = Path.GetExtension(file.FileName);
                    if (extFile == ".xls" || extFile == ".xlsx")
                        //new ConsumptionExcelParser(_ercContext, _branchOfficeService).ParserSecondAsync(file.OpenReadStream()).ForEach(i => list.Add(i));
                        new ConsumptionExcelParser(_ercContext, _branchOfficeService).ParserThirdAsync(file.OpenReadStream()).ForEach(i => list.Add(i));
                });

                return Ok(list);
            }

            return BadRequest(list);
        }
    }
}
