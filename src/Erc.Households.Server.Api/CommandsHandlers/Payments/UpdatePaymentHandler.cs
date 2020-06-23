using AutoMapper;
using Erc.Households.Api.Queries.Payments;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.Payments
{
    public class UpdatePaymentHandler : IRequestHandler<UpdatePayment, Responses.Payment>
    {
        private readonly ErcContext _ercContext;
        private readonly IMapper _mapper;

        public UpdatePaymentHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<Responses.Payment> Handle(UpdatePayment request, CancellationToken cancellationToken)
        {
            var payment = await _ercContext.Payments.Include(t => t.AccountingPoint).FirstOrDefaultAsync(x => x.Id == request.UpdatedPayment.Id);

            if (payment is null)
                return null;

            _ercContext.Entry(payment).CurrentValues.SetValues(request.UpdatedPayment);
            await _ercContext.SaveChangesAsync();

            payment = await _ercContext.Payments.Include(t => t.AccountingPoint).FirstOrDefaultAsync(x => x.Id == request.UpdatedPayment.Id);
            return _mapper.Map<Responses.Payment>(payment);
        }
    }
}
