using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Erc.Households.BranchOfficeManagment.Core;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Api.Controllers
{
    [Route("api/branch-offices")]
    [ApiController]
    [Authorize]
    public class BranchOfficesController : ErcControllerBase
    {
        private readonly IBranchOfficeService _branchOfficeService;
        readonly IMapper _mapper;

        public BranchOfficesController(IBranchOfficeService branchOfficeService, IMapper mapper)
        {
            _branchOfficeService = branchOfficeService ?? throw new ArgumentNullException(nameof(branchOfficeService));
            _mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<Responses.BranchOffice>>(_branchOfficeService.GetList(UserGroups)));
        }

        [HttpPost("{id}/new-period")]
        public IActionResult StartNewPeriod(int id)
        {
            _branchOfficeService.StartNewPeriod(id);
            return Ok();
        }
    }
}