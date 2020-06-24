using Erc.Households.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.BranchOfficeManagment.Core
{
    public interface IBranchOfficeService
    {
        IEnumerable<BranchOffice> GetList(params int[] branchOfficeIds);
        IEnumerable<BranchOffice> GetList(params string[] branchOfficeIds);
        void StartNewPeriod(int branchOfficeId);
    }
}
