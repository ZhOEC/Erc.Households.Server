using Erc.Households.Server.Domain.AccountingPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Billing
{
    public class InvoiceDetail
    {
        public decimal PriceValue { get; set; }
        public int Consumption { get; set; }
        public decimal Sales { get; set; }
        public decimal Kz { get; set; }
        public ZoneNumber ZoneNumber { get; set; }
    }
}
