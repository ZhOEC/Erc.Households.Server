using System;
using System.Collections.Generic;

namespace Erc.Households.Domain.Taxes
{
    public class TaxInvoice
    {
        public int Id { get; set; }
        public DateTime LiabilityDate { get; set; }
        public decimal LiabilitySum { get; set; }
        public decimal EnergyAmount { get; set; }
        public decimal TaxSum { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal FullSum { get; set; }
        public TaxInvoiceType Type { get; set; }
        public int BranchOfficeId { get; set; }
        public IEnumerable<TaxInvoiceTabLine> TabLines { get; private set; }
        public BranchOffice BranchOffice { get; set; }
    }
}
