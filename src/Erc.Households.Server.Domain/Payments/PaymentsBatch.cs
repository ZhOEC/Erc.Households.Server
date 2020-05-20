using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Domain.Payments
{
    public class PaymentsBatch
    {
        public PaymentsBatch()
        {
            Payments = new HashSet<Payment>();
        }

        public PaymentsBatch(int paymentChannelId, ICollection<Payment> payments)
        {
            PaymentChannelId = paymentChannelId;
            Payments = payments;
        }

        public int Id { get; set; }
        public string Name { get; private set; }
        public DateTime IncomingDate { get; private set; }
        public int BranchOfficeId { get; private set; }
        public int PaymentChannelId { get; private set; }
        public virtual ICollection<Payment> Payments { get; private set; }
        public virtual PaymentChannel PaymentChannel { get; set; }
        public bool IsClosed { get; private set; }

        public void Close()
        {
           if (!Payments.All(p => p.Status == PaymentStatus.Processed))
                throw new Exception("Пачка не може бути закрита. Всі платежі повинні бути оброблені");

            IsClosed = true;
        }
    }
}
