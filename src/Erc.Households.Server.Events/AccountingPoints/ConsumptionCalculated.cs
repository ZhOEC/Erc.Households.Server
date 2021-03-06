﻿using System;

namespace Erc.Households.Events.AccountingPoints
{
#pragma warning disable IDE1006 // Naming Styles
    public interface ConsumptionCalculated
#pragma warning restore IDE1006 // Naming Styles
    {
        Guid Id { get; }
        Guid GenerationId { get; }
        string Eic { get; }
        DateTime FromDate { get; }
        int PeriodId { get; } // will be 0
        DateTime ToDate { get; }
        int? PreviousMeterReadingT1 { get; }
        int? PreviousMeterReadingT2 { get; }
        int? PreviousMeterReadingT3 { get; }
        int? PresentMeterReadingT1 { get; }
        int? PresentMeterReadingT2 { get; }
        int? PresentMeterReadingT3 { get; }
        int UsageT1 { get; }
        int? UsageT2 { get; }
        int? UsageT3 { get; }
        string MeterNumber { get; }
        int ZoneRecord { get; } 
        DateTime GenerationDate { get; }
    }
}
