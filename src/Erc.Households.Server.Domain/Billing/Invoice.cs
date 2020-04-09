using Erc.Households.Server.Domain.AccountingPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Billing
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int PreviousMeterReadingT1 { get; set; }
        public int PresentMeterReadingT1 { get; set; }
        public int PreviousMeterReadingT2 { get; set; }
        public int PresentMeterReadingT2 { get; set; }
        public int PreviousMeterReadingT3 { get; set; }
        public int PresentMeterReadingT3 { get; set; }
        public int ConsumptionT1 { get; set; }
        public int ConsumptionT2 { get; set; }
        public int ConsumptionT3 { get; set; }
        public decimal T1Sales { get; set; }
        public decimal T2Sales { get; set; }
        public decimal T3Sales { get; set; }
        public int  TariffId { get; set; }
        public ZoneRecord ZoneRecord { get; set; }
        public Guid DsoConsumptionId { get; set; }
    }
}
