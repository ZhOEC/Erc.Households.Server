using Erc.Households.Domain.AccountingPoints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Erc.Households.Domain.Exemptions
{
    public class AccountingPointExemption
    {
        public int Id { get; private set; }
        public int AccountingPointId { get; private set; }
        public int ExemptionCategoryId { get; private set; }
        public int PersonId { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public string Certificate { get; private set; }
        public bool HasLimit { get; private set; }
        public ExemptionCategory  ExemptionCategory { get; private set; }
        public Person Person { get; private set; }
    }
}
