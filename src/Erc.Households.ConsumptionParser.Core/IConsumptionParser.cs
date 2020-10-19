using Erc.Households.UsageParser.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace Erc.Households.ConsumptionParser.Core
{
    public interface IConsumptionParser
    {
        IReadOnlyCollection<ParsedConsumption> Parse(Stream stream);
    }
}
