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
        public decimal QuantityTotal { get; set; } // Кількість
        public decimal LiabilitySum { get; set; } // База оподаткування
        public decimal TaxSum { get; set; } // Сума ПДВ
        public decimal FullSum { get; init; } // Сума (з ПДВ)
        public DateTime CreationDate { get; set; }
        public TaxInvoiceType Type { get; set; }
        public IEnumerable<TaxInvoiceTabLine> TabLines { get; init; }
        public BranchOffice BranchOffice { get; init; }
        public Period Period { get; init; }
    }
}
