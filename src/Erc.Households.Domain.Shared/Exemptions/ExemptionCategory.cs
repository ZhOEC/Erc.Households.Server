using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Shared.Exemptions
{
    public class ExemptionCategory
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Coeff { get; private set; }
        public bool? HasLimit { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public DateTime? EndDate { get; private set; }
    }
}
