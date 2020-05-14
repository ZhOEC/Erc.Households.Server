using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.Api.Controllers
{
    public abstract class ErcControllerBase : ControllerBase
    {
        protected IEnumerable<string> UserGroups => User.Claims.Where(c => string.Equals(c.Type, "Group", StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value);
    }
}