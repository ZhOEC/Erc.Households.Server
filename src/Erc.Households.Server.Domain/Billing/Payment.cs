using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Billing
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime EnterDate { get; set; }
        public decimal Value { get; set; }
        public int BatchId { get; set; }
    }
}
