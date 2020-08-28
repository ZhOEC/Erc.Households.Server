using Erc.Households.Domain.AccountingPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Billing
{
    public class UsageCalculation
    {
        public decimal PriceValue { get; set; }
        public int Units { get; set; }
        public decimal Charge { get; set; }
        public int DiscountUnits { get; set; }
        public decimal Discount { get; set; }
    }
}
