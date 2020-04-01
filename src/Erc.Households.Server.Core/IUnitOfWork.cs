using System.Threading.Tasks;

namespace Erc.Households.Server.Core
{
    public interface IUnitOfWork
    {
        IAccountingPointRepository AccountingPointRepository { get; }
        Task<int> SaveWorkAsync();
    }
}
