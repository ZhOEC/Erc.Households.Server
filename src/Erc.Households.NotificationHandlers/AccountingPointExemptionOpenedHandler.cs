using Erc.Households.EF.PostgreSQL;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.NotificationHandlers
{
    public class AccountingPointExemptionOpenedHandler : INotificationHandler<AccountingPointExemptionOpened>
    {
        private readonly ErcContext _ercContext;

        public AccountingPointExemptionOpenedHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task Handle(AccountingPointExemptionOpened notification, CancellationToken cancellationToken)
        {
        }
    }
}
