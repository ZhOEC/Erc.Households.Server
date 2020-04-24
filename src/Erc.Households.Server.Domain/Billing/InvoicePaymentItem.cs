using Erc.Households.Server.Domain.Extensions;
using Erc.Households.Server.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Billing
{
    public class InvoicePaymentItem
    {
        Invoice _invoice;
        Payment _payment;

        private Action<object, string> LazyLoader { get; set; }
        protected InvoicePaymentItem(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public InvoicePaymentItem(Invoice invoice, Payment payment, decimal amount)
        {
            _invoice = invoice;
            _payment = payment;
            Amount = amount;
        }

        public int Id { get; private set; }
        public int InvoiceId { get; private set; }
        public int PaymentId { get; private set; }
        public decimal Amount { get; private set; }
        public Invoice Invoice
        {
            get => LazyLoader.Load(this, ref _invoice);
            private set { _invoice = value; }
        }
        public Payment Payment
        {
            get => LazyLoader.Load(this, ref _payment);
            private set { _payment = value; }
        }
    }
}
