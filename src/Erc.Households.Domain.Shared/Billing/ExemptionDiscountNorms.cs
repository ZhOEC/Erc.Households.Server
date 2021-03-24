using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Shared.Billing
{
    public class ExemptionDiscountNorms
    {
        public ExemptionDiscountNorms(DateTime effectiveDate, int baseUnits, int baseUnitsWithoutHotWater, int basePerson, int unitsPerPerson, int maxUnits, int maxUnitsWithoutHotWater, 
            decimal? baseSquareMeter = null, decimal? squareMeterPerPerson = null, int? unitsPerSquareMeter = null)
        {
            EffectiveDate = effectiveDate;
            BaseUnits = baseUnits;
            BaseUnitsWithoutHotWater = baseUnitsWithoutHotWater;
            BasePerson = basePerson;
            UnitsPerPerson = unitsPerPerson;
            MaxUnits = maxUnits;
            MaxUnitsWithoutHotWater = maxUnitsWithoutHotWater;
            BaseSquareMeter = baseSquareMeter;
            SquareMeterPerPerson = squareMeterPerPerson;
            UnitsPerSquareMeter = unitsPerSquareMeter;
        }

        public DateTime EffectiveDate { get; private set; }
        public int BaseUnits { get; private set; }
        public int BaseUnitsWithoutHotWater { get; private set; }
        public int BasePerson { get; private set; } = 1;
        public int UnitsPerPerson { get; private set; }
        public int MaxUnits { get; private set; }
        public int MaxUnitsWithoutHotWater { get; private set; }
        public decimal? BaseSquareMeter { get; private set; }
        public decimal? SquareMeterPerPerson { get; private set; }
        public int? UnitsPerSquareMeter { get; private set; }
    }
}
