using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erc.Households.Domain.Payments
{
    public class PaymentBatch
    {
        public int Id { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalAmount{ get; set; }
        public int ChannelId { get; set; }
        public ICollection<Payment> Payments { get; private set; }
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
