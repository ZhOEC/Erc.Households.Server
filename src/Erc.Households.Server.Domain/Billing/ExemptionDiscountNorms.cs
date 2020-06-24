using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Billing
{
    public class ExemptionDiscountNorms
    {
        public ExemptionDiscountNorms(DateTime effectivetDate, int? baseKWh, int? baseKWhWithoutHotWater, int? basePerson, int? kWhPerPerson, int? maxKWh, int? maxKWhWithoutHotWater, 
            decimal? baseSquareMeter = null, decimal? squareMeterPerPerson = null, int? kWhPerSquareMeter = null)
        {
            EffectivetDate = effectivetDate;
            BaseKWh = baseKWh;
            BaseKWhWithoutHotWater = baseKWhWithoutHotWater;
            BasePerson = basePerson;
            KWhPerPerson = kWhPerPerson;
            MaxKWh = maxKWh;
            MaxKWhWithoutHotWater = maxKWhWithoutHotWater;
            BaseSquareMeter = baseSquareMeter;
            SquareMeterPerPerson = squareMeterPerPerson;
            KWhPerSquareMeter = kWhPerSquareMeter;
        }

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
