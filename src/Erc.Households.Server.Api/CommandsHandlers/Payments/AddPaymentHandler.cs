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
            var accountingPoint = await _ercContext.AccountingPoints.Include(x => x.BranchOffice).FirstOrDefaultAsync(x => x.Id == request.payment.AccountingPointId);
            if (accountingPoint is null)
                return null;

            var payment = new Payment(
                    request.payment.PayDate,
                    request.payment.Amount,
                    accountingPoint.BranchOffice.CurrentPeriodId,
                    PaymentType.CustomerPayment,
                    request.payment.PayerInfo,
                    request.payment.AccountingPointId,
                    request.payment.PaymentsBatchId
                );

            await _ercContext.Payments.AddAsync(payment);
            return _mapper.Map<Responses.Payment>(payment);
        }
    }
}
