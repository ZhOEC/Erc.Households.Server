using Erc.Households.Commands.PaymentBatches;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.CommandHandlers
{
    public class ProcessPaymentBatchHandler : AsyncRequestHandler<ProcessPaymentBatch>
    {
        private readonly ErcContext _ercContext;

        public ProcessPaymentBatchHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        protected override async Task Handle(ProcessPaymentBatch request, CancellationToken cancellationToken)
        {
            var batch = await _ercContext.PaymentBatches
                .Include(pb=>pb.Payments)
                    .ThenInclude(p=>p.AccountingPoint)
                .FirstAsync(pb => pb.Id == request.Id);
            
            batch.ProcessAndClose();
        }
    }
}
