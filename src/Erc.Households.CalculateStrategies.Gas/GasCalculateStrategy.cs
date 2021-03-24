using Erc.Households.CalculateStrategies.Core;
using Erc.Households.Domain.Shared.Billing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.CalculateStrategies.NaturalGas
{
    public class GasCalculateStrategy : ICalculateStrategy
    {
        public Task Calculate(CalculationRequest calculation)
        {
            var tariffRate = calculation.Tariff.Rates.First(tr => tr.StartDate.Date == calculation.FromDate.Date);
            calculation.UsageT1.AddCalculation(new UsageCalculation
            {
                Charge = Math.Round(calculation.UsageT1.Units * tariffRate.Value, 2,  MidpointRounding.AwayFromZero), 
                PriceValue = tariffRate.Value,
                Units = calculation.UsageT1.Units,
            });

            return Task.CompletedTask;
        }
    }
}
