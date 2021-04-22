﻿using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Domain.Payments
{
    public class Payment
    {
        List<InvoicePaymentItem> _invoicePaymentItems = new List<InvoicePaymentItem>();

        public Payment(DateTime payDate, decimal amount, int periodId, PaymentType type, string payerInfo = null, int? accountingPointId = null, string accountingPointName = null, int? batchId = null)
        {
            PayDate = payDate;
            Amount = amount;
            PeriodId = periodId;
            Type = type;
            PayerInfo = payerInfo;
            AccountingPointId = accountingPointId;
            AccountingPointName = accountingPointName;
            BatchId = batchId;

            Status = PaymentStatus.New;
            EnterDate = DateTime.Now;
        }

        protected Payment() { }
        public int Id { get; private set; }
        public int PeriodId { get; private set; }
        public int? BatchId { get; private set; }
        public int? AccountingPointId { get; private set; }
        public DateTime PayDate { get; private set; }
        public DateTime EnterDate { get; private set; }
        public decimal Amount { get; private set; }
        public AccountingPoint AccountingPoint { get; private set; }
        public PaymentsBatch Batch { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string PayerInfo { get; private set; }
        public string AccountingPointName { get; private set; }
        public PaymentType Type { get; private set; }
        public Period Period { get; private set; }
        public decimal Used => _invoicePaymentItems.Sum(p => p.Amount);
        public decimal Balance => Amount - Used;
        public bool IsFullyUsed => Used == Amount; 
        public IEnumerable<InvoicePaymentItem> InvoicePaymentItems => _invoicePaymentItems.AsReadOnly();
    
        public void AddInvoicePaymentItem(InvoicePaymentItem invoicePaymentItem) => _invoicePaymentItems.Add(invoicePaymentItem);

        public void Process(bool updateDebet = true)
        {
            if (AccountingPointId.HasValue)
            {
                AccountingPoint.ProcessPayment(this, updateDebet);
                Status = PaymentStatus.Processed;
            }
            else throw new InvalidPaymentOperationException($"Неможливо обробити платіж {Id}. Абонента не знайдено");
        }

        public void ClearInvoicePaymentItems() => _invoicePaymentItems.Clear();
    }
}
