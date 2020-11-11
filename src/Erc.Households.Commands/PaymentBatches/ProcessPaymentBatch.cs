using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Commands.PaymentBatches
{
    public class ProcessPaymentBatch : IRequest
    {
        public int Id { get; private set; }

        public ProcessPaymentBatch(int id) => Id = id;
    }
}
