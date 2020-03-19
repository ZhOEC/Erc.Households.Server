using System;
using System.Collections.Generic;

namespace Erc.Households.Server.Requests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public interface SearchAccountingPointRequest
    {
        string SearchString { get; }
        IEnumerable<string> BranchOffices { get; }
    }
}
