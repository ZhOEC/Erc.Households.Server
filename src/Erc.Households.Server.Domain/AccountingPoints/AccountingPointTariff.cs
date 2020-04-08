using Erc.Households.Server.Domain.Tariffs;
using Erc.Households.Server.ModelLogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.AccountingPoints
{
    public class AccountingPointTariff: LogableObjectBase
    {
        protected AccountingPointTariff()
        {
            
        }

        public AccountingPointTariff(int tariffId, DateTime date, string user)
        {
            TariffId = tariffId;
            StartDate = date;
            AddLog("create record", user);
        }

        public int Id { get; private set; }
        public DateTime StartDate { get; private set; }
        public int AccountingPointId { get; private set; }
        public int TariffId { get; private set; }
        public Tariff Tariff { get; private set; }
    }
}
