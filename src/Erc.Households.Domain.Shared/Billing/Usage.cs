using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Domain.Shared.Billing
{
    public class Usage
    {
        //protected Usage(decimal units, decimal charge, decimal kz, decimal discountWeight)
        //    :this(units, kz, discountWeight)
        //{
        //    Charge = charge;
        //}

        public Usage(decimal units, decimal kz = 1, decimal discountWeight = 1, decimal charge = 0)
        {
            Units = units;
            Kz = kz;
            DiscountWeight = discountWeight;
            Charge = charge;
        }

        public int? PresentMeterReading { get; init; }
        public int? PreviousMeterReading { get; init; }
        public decimal Units { get; private set; }
        public decimal Charge { get; private set; }
        public decimal Discount { get; private set; }
        public int DiscountUnits { get; private set; }
        public decimal Kz { get; private set; }
        public decimal DiscountWeight { get; private set; }

        private List<UsageCalculation> _calculations = new();
        public IEnumerable<UsageCalculation> Calculations
        {
            get => _calculations;
            set => _calculations = value.ToList();
        }

        public void AddCalculation(UsageCalculation usageCalculation, bool isCorrerctive = false)
        {
            _calculations.Add(usageCalculation);
            Charge += usageCalculation.Charge;
            Discount += usageCalculation.Discount;
            DiscountUnits += usageCalculation.DiscountUnits;
            if (isCorrerctive)
            {
                Units += usageCalculation.Units;
            }
        }

        public void AddInvalidInvoicesUnits(decimal units) => Units += units;
    }
}
