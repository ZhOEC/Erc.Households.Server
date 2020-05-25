using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Billing
{
    public class Usage
    {
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

        public int PresentMeterReading { get; private set; }
        public int PreviousMeterReading { get; private set; }
        public int Units { get; private set; }
        public decimal Charge { get; private set; }
        public decimal Discount { get; private set; }
        public decimal DiscountUnits { get; private set; }
        public decimal Kz { get; private set; }
        public decimal DiscountWeight { get; private set; }
        public IEnumerable<UsageCalculation> Calculations { get; private set; }
    }
}
