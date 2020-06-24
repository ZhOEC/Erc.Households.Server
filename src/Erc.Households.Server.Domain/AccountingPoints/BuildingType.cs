using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.AccountingPoints
{
    public class BuildingType
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal HeataingCorrection { get; private set; }
    }
}
