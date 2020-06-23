using AutoMapper;
using Erc.Households.Api.Queries.Payments;
using Erc.Households.Domain.Payments;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.Payments
{
    public class AddPaymentHandler : IRequestHandler<AddPayment, Responses.Payment>
    {
        private readonly ErcContext _ercContext;
        private readonly IMapper _mapper;

        public AddPaymentHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;

        }

        public async Task<Responses.Payment> Handle(AddPayment request, CancellationToken cancellationToken)
        {
            var accountingPoint = await _ercContext.AccountingPoints.Include(x => x.BranchOffice).FirstOrDefaultAsync(x => x.Id == request.Payment.AccountingPointId);
            if (accountingPoint is null)
                return null;

            var payment = new Payment(
                    request.Payment.PayDate,
                    request.Payment.Amount,
                    accountingPoint.BranchOffice.CurrentPeriodId,
                    request.Payment.Type,
                    request.Payment.PayerInfo,
                    request.Payment.AccountingPointId,
                    null,
                    request.Payment.BatchId
                );

            await _ercContext.Payments.AddAsync(payment);
            await _ercContext.SaveChangesAsync();
            return _mapper.Map<Responses.Payment>(payment);
        }
    }
}
