using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Taxes;
using System;
using System.Collections.Generic;

namespace Erc.Households.Api.Responses
{
    public class TaxInvoice
    {
        public int Id { get; set; }
        public int BranchOfficeId { get; set; }
        public int PeriodId { get; init; }
        public DateTime LiabilityDate { get; set; }
        public decimal QuantityTotal { get; set; }
        public decimal LiabilitySum { get; set; }
        public decimal TaxSum { get; set; }
        public decimal FullSum { get; init; }
        public DateTime CreationDate { get; set; }
        public TaxInvoiceType Type { get; set; }
        public IEnumerable<TaxInvoiceTabLine> TabLines { get; init; }
        public BranchOffice BranchOffice { get; init; }
        public Period Period { get; init; }
    }
}
