using Erc.Households.Domain.Billing;
using MediatR;

namespace Erc.Households.Commands
{
    public class CreateTaxInvoiceCommand : IRequest
    {
        public CreateTaxInvoiceCommand(int branchOfficeId, int periodId)
        {
            PeriodId = periodId;
            BranchOfficeId = branchOfficeId;
        }

        public int PeriodId { get; private set; }
        public int BranchOfficeId { get; private set; }
    }
}
