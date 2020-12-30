using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Commands
{
    public class CalculateAccountingPoint
    {
        public Guid Id { get; private set; }
        public Guid GenerationId { get; private set;}
        public string Eic { get; private set;}
        public DateTime? FromDate { get; private set;}
        public DateTime? ToDate { get; private set;}
        public int? PreviousMeterReadingT1 { get; private set;}
        public int? PreviousMeterReadingT2 { get; private set;}
        public int? PreviousMeterReadingT3 { get; private set;}
        public int? PresentMeterReadingT1 { get; private set;}
        public int? PresentMeterReadingT2 { get; private set;}
        public int? PresentMeterReadingT3 { get; private set;}
        public decimal UsageT1 { get; private set;}
        public int? UsageT2 { get; private set;}
        public int? UsageT3 { get; private set;}
        public string MeterNumber { get; private set;}
        public int ZoneRecord { get; private set;}
        public DateTime GenerationDate { get; private set;}
        public DateTime PeriodDate { get; private set; }

        /// <summary>
        /// Conversion factor to standard conditions.
        /// It uses for natural gas calculation only
        /// </summary>
        public decimal Ksc { get; private set; } = 1;
    }
}
