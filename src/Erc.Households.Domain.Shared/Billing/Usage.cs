using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Domain.Shared.Billing
{
    public class Usage
    {
        private List<UsageCalculation> _calculations = new List<UsageCalculation>();
        public Usage(decimal units, decimal kz)
        {
            Units = units;
            Kz = kz;
        }

        public int? PresentMeterReading { get; init; }
        public int? PreviousMeterReading { get; init; }
        public decimal Units { get; private set; }
        public decimal Charge { get; private set; }
        public decimal Discount { get; private set; }
        public int DiscountUnits { get; private set; }
        public decimal Kz { get; private set; } 
        public decimal DiscountWeight { get; private set; }
        public ZoneNumber Zone { get; private set; }
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
            if (isCorrerctive)
                Units += usageCalculation.Units;
        }

        public void AddInvalidInvoicesUnits(decimal units) => Units += units;
    }
}
