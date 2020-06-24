using System;

namespace Erc.Households.Commands
{
    public class CloseAccountingPointExemption: MediatR.IRequest
    {
        public CloseAccountingPointExemption(int accountingPointId, DateTime date, string note)
        {
            AccountingPointId = accountingPointId;
            Date = date;
            Note = note;
        }

        public int AccountingPointId { get; private set; }
        public DateTime Date { get; private set; }
        public string Note { get; private set; }
    }
}
