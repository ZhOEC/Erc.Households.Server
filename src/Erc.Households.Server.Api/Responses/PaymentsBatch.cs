﻿using System;

namespace Erc.Households.Api.Responses
{
    public class PaymentsBatch
    {
        public int Id { get; set; }
        public string PaymentChannelName { get; set; }
        public string Name { get; set; }
        public DateTime IncomingDate { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentChannelId { get; set; }
        public bool IsClosed { get; set; }
    }
}
