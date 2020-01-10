using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Backend.Data.Tariffs
{
    public class TariffRate
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; } 
        public decimal Value { get; set; }
        public int? ConsumptionLimit { get; set; }
        public int? HeatingConsumptionLimit { get; set; }
        public int? HeatingStartDay { get; set; }
        public int? HeatingStartMonth { get; set; }
        public int? HeatingEndDay { get; set; }
        public int? HeatingEndMonth { get; set; }
        public int TariffId { get; set; }
    }
}
