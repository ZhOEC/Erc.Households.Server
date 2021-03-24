using Erc.Households.Domain.Extensions;
using Erc.Households.Domain.Shared;
using Erc.Households.ModelLogs;
using System;

namespace Erc.Households.Domain.AccountingPoints
{
    public class Contract : LogableObjectBase
    {
        Person _customer;
        private Action<object, string> LazyLoader { get; set; }

        protected Contract(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public Contract(DateTime startDate, Person customer, string openUser, bool sendPaperBill)
        {
            StartDate = startDate;
            if (customer.Id == 0)
                Customer = customer;
            else
                CustomerId = customer.Id;
            SendPaperBill = sendPaperBill;
            AddLog("open", openUser);
        }

        public int Id { get; private set; }
        public int AccountingPointId { get; private set; }
        public int CustomerId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public bool SendPaperBill { get; private set; }
        public Person Customer 
        {
            get => LazyLoader.Load(this, ref _customer);
            private set { _customer = value; }
        }
    
        public bool IsActive => !EndDate.HasValue;

        public void Close(DateTime closeDate, string closeUser)
        {
            EndDate = closeDate;
            AddLog("close", closeUser);
        }
    }
}
