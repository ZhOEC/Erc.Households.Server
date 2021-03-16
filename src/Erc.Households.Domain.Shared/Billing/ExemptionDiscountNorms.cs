using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Shared.Billing
{
    public class ExemptionDiscountNorms
    {
        public ExemptionDiscountNorms(DateTime effectivetDate, int? baseUnits, int? baseUnitsWithoutHotWater, int? basePerson, int? unitsPerPerson, int? maxUnits, int? maxUnitsWithoutHotWater, 
            decimal? baseSquareMeter = null, decimal? squareMeterPerPerson = null, int? unitsPerSquareMeter = null)
        {
            EffectivetDate = effectivetDate;
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

        public DateTime EffectivetDate { get; private set; }
        public int? BaseUnits { get; private set; }
        public int? BaseUnitsWithoutHotWater { get; private set; }
        public int? BasePerson { get; private set; }
        public int? UnitsPerPerson { get; private set; }
        public int? MaxUnits { get; private set; }
        public int? MaxUnitsWithoutHotWater { get; private set; }
        public decimal? BaseSquareMeter { get; private set; }
        public decimal? SquareMeterPerPerson { get; private set; }
        public int? UnitsPerSquareMeter { get; private set; }
    }
}
