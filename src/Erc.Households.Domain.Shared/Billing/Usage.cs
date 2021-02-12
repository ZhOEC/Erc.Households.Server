using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Domain.Shared.Billing
{
    public class Usage
    {
        private List<UsageCalculation> _calculations = new List<UsageCalculation>();
        public Usage(decimal units, decimal k, ZoneNumber zone)
        {
            Units = units;
            Kz = k;
            Zone = zone;
        }

        public int? PresentMeterReading { get; init; }
        public int? PreviousMeterReading { get; init; }
        public decimal Units { get; private set; }
        public decimal Charge { get; set; }
        public decimal Discount { get; set; }
        public int DiscountUnits { get; set; }
        public decimal Kz { get; init; } = 1;
        public decimal DiscountWeight { get; set; }
        public ZoneNumber Zone { get; private set; }
        public IEnumerable<UsageCalculation> Calculations 
        { 
            get => _calculations; 
            set => _calculations = value.ToList(); 
        }

        public void AddCalculation(UsageCalculation usageCalculation)
        {
            _calculations.Add(usageCalculation);
            Charge += usageCalculation.Charge;
            Discount += usageCalculation.Discount;
        }

        public void AddInvalidInvoicesUnits(decimal units) => Units += units;
    }
}
