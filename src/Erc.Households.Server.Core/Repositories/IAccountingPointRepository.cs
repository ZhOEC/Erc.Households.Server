using Erc.Households.Domain.AccountingPoints;
using System;
using System.Threading.Tasks;

namespace Erc.Households.Core
{
    public interface IAccountingPointRepository
    {
        Task<AccountingPoint> GetAsync(int id);
        Task<AccountingPoint> GetAsync(string eic);
        Task AddNewAsync(AccountingPoint accountingPoint);
    }
}
