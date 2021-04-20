using Erc.Households.Domain.Taxes;
using System;

namespace Erc.Households.Api.Responses
{
    public class TaxInvoice
    {
        public int Id { get; set; }
        public TaxInvoiceType Type { get; set; }
        public DateTime LiabilityDate { get; set; }
        public decimal LiabilitySum { get; set; }
        public decimal TariffValue { get; set; }
        public decimal EnergyAmount { get; set; }
        public decimal TaxSum { get; set; }
        public DateTime CreationDate { get; set; }
        public int BranchOfficeId { get; set; }
    }
}
