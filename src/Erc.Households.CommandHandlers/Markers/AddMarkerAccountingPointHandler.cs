using Erc.Households.Commands;
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.CommandHandlers
{
    class AddMarkerAccountingPointHandler : AsyncRequestHandler<AddMarkerAccountingPointCommand>
    {
        private readonly ErcContext _ercContext;

        public AddMarkerAccountingPointHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        protected override async Task Handle(AddMarkerAccountingPointCommand request, CancellationToken cancellationToken)
        {
            var ap = await _ercContext.AccountingPoints.FindAsync(request.AccountingPointId);
            if (ap is null)
                throw new Exception("Accounting point not exist");

            var marker = new AccountingPointMarker(request.AccountingPointId, request.MarkerId, request.Note);
            ap.AddMarker(marker);
        }
    }
}
