using System;
using MediatR;
using Erc.Households.Commands.Markers;
using Erc.Households.EF.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Erc.Households.CommandHandlers.Markers
{
    public class DeleteMarkerAccountingPointHandler : AsyncRequestHandler<DeleteMarkerAccountingPointCommand>
    {
        private readonly ErcContext _ercContext;

        public DeleteMarkerAccountingPointHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        protected override async Task Handle(DeleteMarkerAccountingPointCommand request, CancellationToken cancellationToken)
        {
            var ap = await _ercContext.AccountingPoints.FirstOrDefaultAsync(x => x.Id == request.AccountingPointId);
            if (ap is null) 
                throw new Exception("Accounting point not exist");

            var marker = ap.Markers.FirstOrDefault(x => x.Id == request.MarkerId);
            if (marker is null)
                throw new Exception("Marker not exist");

            ap.RemoveMarker(marker);
        }
    }
}
