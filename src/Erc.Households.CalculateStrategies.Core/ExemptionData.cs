using Erc.Households.Domain.Shared.Billing;
using System.Collections.Generic;

namespace Erc.Households.CalculateStrategies.Core
{
    public record ExemptionData
    {
        public IEnumerable<ExemptionDiscountNorms> ExemptionDiscountNorms { get; init; }
        public decimal ExemptionPercent { get; init; }
        public decimal HeatingCorrection { get; init; }
        public int NumberOfPeople { get; init; }
        public bool IsGasWaterHeaterInstalled { get; init; }
        public bool CanBeUsedElectricWaterHeater { get; init; }
        public bool IsCentralizedHotWaterSupply { get; init; }
        public bool IsElectricHeatig { get; init; }
        public bool UseDiscountLimit { get; init; }
    }
}
