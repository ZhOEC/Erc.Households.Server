using System.Threading.Tasks;

namespace Erc.Households.DataAccess.Core
{
    public interface IUnitOfWork
    {
        IAccountingPointRepository AccountingPointRepository { get; }
        Task<int> SaveWorkAsync();
    }
}
