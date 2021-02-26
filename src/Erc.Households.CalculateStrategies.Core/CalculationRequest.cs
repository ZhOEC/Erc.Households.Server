﻿using Erc.Households.Domain.Shared;
using Erc.Households.Domain.Shared.Billing;
using Erc.Households.Domain.Shared.Tariffs;
using System;
using System.Collections.Generic;

namespace Erc.Households.CalculateStrategies.Core
{
    public record CalculationRequest
    {
        public int AccountingPointId { get; init; }
        public IEnumerable<(Usage invalidUsageT1, Usage invalidUsageT2, Usage invalidUsageT3)> InvalidUsages { get; init; }
        public DateTime FromDate { get; init; }
        public DateTime ToDate { get; init; }
        public Usage UsageT1 { get; init; }
        public Usage UsageT2 { get; init; }
        public Usage UsageT3 { get; init; }
        public Tariff Tariff { get; init; }
        public InvoiceType InvoiceType { get; init; }
        public ZoneRecord ZoneRecord { get; init; }
    }
}
