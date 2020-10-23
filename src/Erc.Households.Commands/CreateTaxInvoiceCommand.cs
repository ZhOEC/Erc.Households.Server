using Erc.Households.Domain.Billing;
using MediatR;

namespace Erc.Households.Commands
{
    public class CreateTaxInvoiceCommand : IRequest
    {
        public CreateTaxInvoiceCommand(Period currentPeriod)
        {
            CurrentPeriod = currentPeriod;
        }

        public Period CurrentPeriod { get; set; }
    }
}
