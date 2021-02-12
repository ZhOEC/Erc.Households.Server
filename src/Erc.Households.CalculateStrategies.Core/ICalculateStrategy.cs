using System.Threading.Tasks;

namespace Erc.Households.CalculateStrategies.Core
{
    public interface ICalculateStrategy
    {
        Task Calculate(CalculationRequest calculation);
    }
}
