﻿using System;

namespace Erc.Households.Domain.Shared.Tariffs
{
    public class TariffRate
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; } 
        public decimal Value { get; set; }
        public int? ConsumptionLimit { get; set; }
        public int? HeatingConsumptionLimit { get; set; }
        public DateTime? HeatingStartDay { get; set; }
        public DateTime? HeatingEndDay { get; set; }
    }
}
