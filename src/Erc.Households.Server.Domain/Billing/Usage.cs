using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erc.Households.Domain.Billing
{
    public class Usage
    {
        private List<UsageCalculation> _calculations = new List<UsageCalculation>();

        public int? PresentMeterReading { get; set; }
        public int? PreviousMeterReading { get; set; }
        public int Units { get; set; }
        public decimal Charge { get; set; }
        public decimal Discount { get; set; }
        public int DiscountUnits { get; set; }
        public decimal Kz { get; set; }
        public decimal DiscountWeight { get; set; }
        public IEnumerable<UsageCalculation> Calculations 
        { 
            get => _calculations; 
            set => _calculations = value.ToList(); 
        }

        public void AddCalculation(UsageCalculation usageCalculation)
        {
            _calculations.Add(usageCalculation);
            //Charge += usageCalculation.Charge;
            //Discount += usageCalculation.Discount;
        }
    }
}
