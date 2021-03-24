namespace Erc.Households.Domain.Shared.Billing
{
    public class UsageCalculation
    {
        public decimal PriceValue { get; init; }
        public decimal Units { get; init; }
        public decimal Charge { get; init; }
        public int DiscountUnits { get; init; }
        public decimal Discount { get; init; }
    }
}
