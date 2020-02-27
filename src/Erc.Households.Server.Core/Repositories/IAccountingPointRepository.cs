using Erc.Households.Server.Domain.AccountingPoints;
using System;
using System.Threading.Tasks;

namespace Erc.Households.Server.Core
{
    public interface IAccountingPointRepository
    {
        Task<AccountingPoint> GetAsync(int id);
        Task<AccountingPoint> GetAsync(string eic);
        Task AddNewAsync(AccountingPoint accountingPoint);
    }
}
