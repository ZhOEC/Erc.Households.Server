using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.Domain.Payments
{
    public class PaymentBatch
    {
        public PaymentBatch()
        {
            Payments = new HashSet<Payment>();
        }

        public PaymentBatch(int channelId, ICollection<Payment> payments)
        {
            ChannelId = channelId;
            Payments = payments;
        }

        public int Id { get; set; }
        public string Name => $"Пачка №{Id}";
        public DateTime Date => DateTime.Now;
        public int TotalCount { get; set; }
        public decimal TotalAmount{ get; set; }
        public int ChannelId { get; private set; }
        public virtual ICollection<Payment> Payments { get; private set; }
        public bool IsChecked => (Payments.Sum(p => p.Amount) == TotalAmount) && (Payments.Count() == TotalCount);
        public bool IsClosed { get; private set; }

        public void Close()
        {
            if (!IsChecked)
                throw new Exception("Пачка не може бути закрита. Сумма та кількість платежів повинна співпадати с заявленою");

            if (!Payments.All(p => p.Status == PaymentStatus.Processed))
                throw new Exception("Пачка не може бути закрита. Всі платежі повинні бути оброблені");

            IsClosed = true;
        }
    }
}
