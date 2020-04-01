using Erc.Households.Server.ModelLogs;
using System;

namespace Erc.Households.Server.Domain.AccountingPoints
{
    public class Contract: LogableObjectBase
    {
        protected Contract()
        {

        }

        public Contract(DateTime startDate, Person customer, string openUser)
        {
            StartDate = startDate;
            Customer = customer;
            AddLog("open", openUser);
        }

        public int Id { get; private set; }
        public int AccountingPointId { get; private set; }
        public int CustomerId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public Person Customer { get; private set; }
        public bool IsActive => !EndDate.HasValue;

        public void Close(DateTime closeDate, string closeUser)
        {
            EndDate = closeDate;
            AddLog("close", closeUser);
        }
    }
}
