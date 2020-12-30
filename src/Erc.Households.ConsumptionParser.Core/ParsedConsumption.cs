using System;

namespace Erc.Households.UsageParser.Core
{
    public class ParsedConsumption
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid GenerationId { get; set; }
        public string Eic { get; set; }
        public int? PreviousMeterReadingT1 { get; set; }
        public int? PreviousMeterReadingT2 { get; set; }
        public int? PreviousMeterReadingT3 { get; set; }
        public int? PresentMeterReadingT1 { get; set; }
        public int? PresentMeterReadingT2 { get; set; }
        public int? PresentMeterReadingT3 { get; set; }
        public DateTime PeriodDate { get; set; }
        public decimal UsageT1 { get; set; }
        public int? UsageT2 { get; set; }
        public int? UsageT3 { get; set; }
        public DateTime GenerationDate { get; private set; } = DateTime.Now;
        public bool IsParsesd { get; set; }
        public int RowNumber { get; set; }
        public string ErrorMessage { get; set; }
    }
}
