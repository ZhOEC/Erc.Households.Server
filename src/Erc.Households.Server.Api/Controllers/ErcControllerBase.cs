using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.Server.Api.Controllers
{
    public abstract class ErcControllerBase : ControllerBase
    {
        protected IEnumerable<string> UserGroups => User.Claims.Where(c => string.Equals(c.Type, "Group", StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value);
    }
}