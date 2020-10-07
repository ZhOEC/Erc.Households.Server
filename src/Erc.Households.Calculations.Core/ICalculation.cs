using Erc.Households.Domain.Shared.Tariffs;
using System;

namespace Erc.Households.Calculations.Core
{
    public interface ICalculation
    {
        DateTime FromDate { get; }
        DateTime ToDate { get; }
        Usage UsageT1 { get; set; }
        Usage UsageT2 { get; set; }
        Usage UsageT3 { get; set; }
        Tariff Tariff { get; }
        //void Calculate(); 
    }
}
