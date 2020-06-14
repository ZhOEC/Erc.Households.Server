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

        public PaymentsBatch(int branchOfficeId, int paymentChannelId, DateTime incomingDate, ICollection<Payment> payments)
        {
            BranchOfficeId = branchOfficeId;
            PaymentChannelId = paymentChannelId;
            IncomingDate = incomingDate;
            Payments = payments;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime IncomingDate { get; set; }
        public int BranchOfficeId { get; set; }
        public int PaymentChannelId { get; set; }
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
