using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Billing
{
    public class Usage
    {
        protected Usage() { }

        public Usage(int presentMeterReading, int previousMeterReading, int units, decimal kz = 1, decimal discountWeight = 1 /*, decimal charge, decimal kz = 1, decimal discountWeight = 1, int discountUnits = 0, decimal discount = 0*/)
        {
            PresentMeterReading = presentMeterReading;
            PreviousMeterReading = previousMeterReading;
            Units = units;
            //Charge = charge;
            //Discount = discount;
            //DiscountUnits = discountUnits;
            Kz = kz;
            DiscountWeight = discountWeight;
        }

        public int PresentMeterReading { get; set; }
        public int PreviousMeterReading { get; set; }
        public int Units { get; set; }
        public decimal Charge { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountUnits { get; set; }
        public decimal Kz { get; set; }
        public decimal DiscountWeight { get; set; }
        public IEnumerable<UsageCalculation> Calculations { get; set; } = new HashSet<UsageCalculation>();
    }
}
