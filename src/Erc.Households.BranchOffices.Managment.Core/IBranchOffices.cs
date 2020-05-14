using Erc.Households.Domain;
using System.Collections.Generic;

namespace Erc.Households.BranchOfficeManagment.Core
{
    public interface IBranchOffices
    {
        IEnumerable<BranchOffice> GetList(params int[] branchOfficeIds);
        IEnumerable<BranchOffice> GetList(params string[] branchOfficeIds);
        void StartNewPeriod(int branchOfficeId);
    }
}