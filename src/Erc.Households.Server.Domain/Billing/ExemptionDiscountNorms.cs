using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Billing
{
    public class ExemptionDiscountNorms
    {
        public DateTime EffectivetDate { get; private set; }
        public int? BaseKWh { get; private set; }
        public int? BaseKWhWithoutHotWater { get; private set; }
        public int? BasePerson { get; private set; }
        public int? KWhPerPerson { get; private set; }
        public int? MaxKWh { get; private set; }
        public int? MaxKWhWithoutHotWater { get; private set; }
        public decimal? BaseSquareMeter { get; private set; }
        public decimal? SquareMeterPerPerson { get; private set; }
        public int? KWhPerSquareMeter { get; private set; }
    }
}
