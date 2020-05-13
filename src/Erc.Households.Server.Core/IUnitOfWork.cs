using System.Threading.Tasks;

namespace Erc.Households.Core
{
    public interface IUnitOfWork
    {
        IAccountingPointRepository AccountingPointRepository { get; }
        Task<int> SaveWorkAsync();
    }
}
