using Erc.Households.Domain.AccountingPoints;
using MediatR;
using System;

namespace Erc.Households.Notifications
{
    public class AccountingPointExemptionClosed : INotification
    {
        public AccountingPointExemption Exemption { get; }

        public AccountingPointExemptionClosed(AccountingPointExemption exemption)
        {
            Exemption = exemption;
        }
    }
}
