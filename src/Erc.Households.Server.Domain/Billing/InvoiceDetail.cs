using Erc.Households.Domain.AccountingPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Billing
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal PriceValue { get; set; }
        public int Consumption { get; set; }
        public decimal Sales { get; set; }
        public decimal Kz { get; set; }
        public ZoneNumber ZoneNumber { get; set; }
    }
}
