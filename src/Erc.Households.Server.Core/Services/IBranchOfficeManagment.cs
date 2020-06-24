using Erc.Households.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Core.Services
{
    public interface IBranchOfficeManagment
    {
        IEnumerable<BranchOffice> GetList(params int[] branchOfficeIds);
        IEnumerable<BranchOffice> GetList(params string[] branchOfficeIds);
        void StartNewPeriod(int branchOfficeId);
    }
}
