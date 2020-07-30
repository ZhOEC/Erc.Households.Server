using Erc.Households.Domain.AccountingPoints;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.NotificationHandlers
{
    public class OpenAccountingPointExemptionCommandHandler : AsyncRequestHandler<OpenAccountingPointExemptionCommand>
    {
        private readonly ErcContext _ercContext;
        private readonly IMediator _mediator;

        public OpenAccountingPointExemptionCommandHandler(ErcContext ercContext, IMediator mediator)
        {
            _ercContext = ercContext;
            _mediator = mediator;
        }

        protected override async Task Handle(OpenAccountingPointExemptionCommand request, CancellationToken cancellationToken)
        {
            var ap = await _ercContext.AccountingPoints.FindAsync(request.AccountingPointId);
            if (ap is null)
                throw new Exception("Accounting point not exist");

            if (ap.Exemption is null)
            {
                var exemption = new AccountingPointExemption();
                exemption.Open(request.AccountingPointId, request.CategoryId, request.Date, request.Certificate, request.PersonCount, request.Limit, request.Note, request.Person);

                ap.SetExemption(exemption);
                _ercContext.Entry(ap.Exemption.Person).State = exemption.Person.Id == 0 ? EntityState.Added : EntityState.Modified;

                await _mediator.Publish(new AccountingPointExemptionOpened(ap.Exemption?.Id ?? 0, request.AccountingPointId, request.CategoryId, request.Date,
                    request.Certificate, request.PersonCount, request.Limit, request.Note, request.Person));
            } else throw new Exception("Accounting point have current exemption");
        }
    }
}
