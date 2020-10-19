using Erc.Households.Domain.Shared.Tariffs;
using System;

namespace Erc.Households.CalculateStrategies.Core
{
    public class CalculationRequest
    {
        public CalculationRequest(DateTime fromDate, DateTime toDate, Usage usageT1, Usage usageT2, Usage usageT3, Tariff tariff)
        {
            FromDate = fromDate;
            ToDate = toDate;
            UsageT1 = usageT1;
            UsageT2 = usageT2;
            UsageT3 = usageT3;
            Tariff = tariff;
        }

        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public Usage UsageT1 { get; private set; }
        public Usage UsageT2 { get; private set; }
        public Usage UsageT3 { get; private set; }
        public Tariff Tariff { get; private set; }
    }
}
