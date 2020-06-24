﻿using Erc.Households.Api.Queries.Payments;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.Payments
{
    public class DeletePaymentHandler : IRequestHandler<DeletePaymentCommand, Unit>
    {
        private readonly ErcContext _ercContext;

        public DeletePaymentHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<Unit> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _ercContext.Payments.FindAsync(request.Id);
            _ercContext.Remove(payment);

            return await _ercContext.SaveChangesAsync() > 0 ? Unit.Value : throw new Exception("Can't remove payment");
        }
    }
}
