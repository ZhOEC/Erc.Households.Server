using Erc.Households.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain
{
    public class DistributionSystemOperator
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Commodity Commodity { get; private set; }
    }
}
