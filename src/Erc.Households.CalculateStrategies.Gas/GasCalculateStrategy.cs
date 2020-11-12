using Erc.Households.CalculateStrategies.Core;
using System;
using System.Linq;

namespace Erc.Households.CalculateStrategies.NaturalGas
{
    public class GasCalculateStrategy : ICalculateStrategy
    {
        public void Calculate(CalculationRequest calculation)
        {
            var tariffRate = calculation.Tariff.Rates.First(tr => tr.StartDate.Date == calculation.FromDate.Date);
            calculation.UsageT1.AddCalculation(new UsageCalculation
            {
                Charge = Math.Round(calculation.UsageT1.Units * tariffRate.Value, 2,  MidpointRounding.AwayFromZero), 
                PriceValue = tariffRate.Value,
                Units = calculation.UsageT1.Units,
            });
        }
    }
}
