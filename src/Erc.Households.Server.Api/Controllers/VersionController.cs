using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        public IActionResult GetVersion()
        {

            return Ok(new
            {
                ServerTime = DateTime.Now.ToString("F")
            });
        }
    }
}