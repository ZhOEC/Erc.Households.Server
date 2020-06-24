using Erc.Households.Commands;
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Notifications;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.CommandHandlers
{
    public class CloseAccountingPointExemptionHandler : AsyncRequestHandler<CloseAccountingPointExemption>
    {
        private readonly ErcContext _ercContext;
        private readonly IMediator _mediator;

        public CloseAccountingPointExemptionHandler(ErcContext ercContext, IMediator mediator)
        {
            _ercContext = ercContext;
            _mediator = mediator;
        }

        protected override async Task Handle(CloseAccountingPointExemption request, CancellationToken cancellationToken)
        {
            var ac = await _ercContext.AccountingPoints.FindAsync(request.AccountingPointId);
            if (ac.Exemption != null)
            {
                ac.Exemption?.Close(request.Date, request.Note);
                await _mediator.Publish(new AccountingPointExemptionClosed(ac?.Exemption.Id ?? 0, request.AccountingPointId, request.Date));
            }
        }
    }
}
