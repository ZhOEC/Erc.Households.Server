using MediatR;
using System;

namespace Erc.Households.Notifications
{
    public class AccountingPointExemptionClosed : INotification
    {
        public int Id { get; }
        public int AccountingPointId { get; }
        public DateTime Date { get; }

        public AccountingPointExemptionClosed(int id, int accountingPointId, DateTime date)
        {
            Id = id;
            AccountingPointId = accountingPointId;
            Date = date;
        }
    }
}
