using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Billing
{
    public class InvoicePaymentItem
    {
        public int Id { get; private set; }
        public int InvoiceId { get; private set; }
        public int PaymentId { get; private set; }
        public decimal Value { get; private set; }
        public Invoice Invoice { get; private set; }
        public Payment Payment { get; private set; }
    }
}
