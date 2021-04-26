using Erc.Households.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Domain.Payments
{
    public class PaymentsBatch
    {
        private Action<object, string> LazyLoader { get; set; }
        private List<Payment> _payments = new List<Payment>();
        
        private PaymentsBatch(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public PaymentsBatch(int branchOfficeId, int paymentChannelId, DateTime incomingDate, ICollection<Payment> payments)
        {
            BranchOfficeId = branchOfficeId;
            PaymentChannelId = paymentChannelId;
            IncomingDate = incomingDate;
            Payments = payments;
        }

        public int Id { get; set; }
        public string Name => Id.ToString();
        public DateTime IncomingDate { get; set; }
        public int BranchOfficeId { get; set; }
        public int PaymentChannelId { get; set; }
        public IEnumerable<Payment> Payments
        {
            get => LazyLoader.Load(this, ref _payments);
            private set { _payments = value.ToList(); }
        }
        //public IEnumerable<Payment> Payments { get; private set; }
        public BranchOffice BranchOffice { get; private set; }
        public PaymentChannel PaymentChannel { get; set; }
        public bool IsClosed { get; private set; }

        public void Close()
        {
           if (!Payments.All(p => p.Status == PaymentStatus.Processed))
                throw new Exception("Пачка не може бути закрита. Всі платежі повинні бути оброблені");

            IsClosed = true;
        }

        public void ProcessAndClose()
        {
            foreach (var payment in Payments)
                payment.Process();

            Close();
        }
    }
}
