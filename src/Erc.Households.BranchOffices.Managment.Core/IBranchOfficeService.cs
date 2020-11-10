using Erc.Households.Domain;
using Erc.Households.Domain.Billing;
using System.Collections.Generic;

namespace Erc.Households.BranchOfficeManagment.Core
{
    public interface IBranchOfficeService
    {
        IEnumerable<BranchOffice> GetList(params int[] branchOfficeIds);
        IEnumerable<BranchOffice> GetList(IEnumerable<string> branchOfficeIds);
        BranchOffice GetOne(int id);
        IEnumerable<Period> GetPeriods(int Id);
        void StartNewPeriod(int branchOfficeId);
    }
}