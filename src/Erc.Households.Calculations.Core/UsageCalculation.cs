namespace Erc.Households.Calculations.Core
{
    public class UsageCalculation
    {
        public decimal PriceValue { get; set; }
        public decimal Units { get; set; }
        public decimal Charge { get; set; }
        public int DiscountUnits { get; set; }
        public decimal Discount { get; set; }
    }
}
