using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Erc.Households.Api.Controllers
{
    public abstract class ErcControllerBase : ControllerBase
    {
        protected string UserId => User.Claims.FirstOrDefault(c => c.Properties.Values.Contains("sub")).Value;

        protected IEnumerable<string> UserGroups 
            => User.Claims.Where(c => string.Equals(c.Type, "Group", StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value);
    }
}