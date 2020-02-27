using System.Threading.Tasks;

namespace Erc.Households.Server.Core
{
    public interface IUnitOfWork
    {
       Task<int> SaveWorkAsync();
    }
}
