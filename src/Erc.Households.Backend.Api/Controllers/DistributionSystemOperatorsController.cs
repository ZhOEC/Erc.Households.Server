﻿using System;
using System.Threading.Tasks;
using Erc.Households.Backend.DataAccess.PostgreSql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Erc.Households.Backend.Api.Controllers
{
    [Route("api/distribution-system-operators")]
    [ApiController]
    public class DistributionSystemOperatorsController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public DistributionSystemOperatorsController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll() => Ok(await _ercContext.DistributionSystemOperators.ToListAsync());
    }
}