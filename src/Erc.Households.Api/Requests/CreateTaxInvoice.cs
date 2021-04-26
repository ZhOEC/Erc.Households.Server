using Erc.Households.Domain;
using Erc.Households.Domain.Billing;

namespace Erc.Households.Api.Requests
{
    public class CreateTaxInvoice
    {
        public BranchOffice BranchOffice { get; init; }
        public Period Period { get; init; }
        public bool IsDisabled { get; init; }
    }
}
