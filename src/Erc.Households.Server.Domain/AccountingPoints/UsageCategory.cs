using System.Collections.Generic;

namespace Erc.Households.Domain.AccountingPoints
{
    public class UsageCategory
    {
        private UsageCategory() { }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<Billing.ExemptionDiscountNorms> ExemptionDiscountNorms { get; private set; }
    }
}