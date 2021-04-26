using Erc.Households.Domain.Taxes;
using MediatR;
using System;
using System.Collections.Generic;

namespace Erc.Households.Commands
{
    public record CreateTaxInvoiceCommand : IRequest
    {
        public int BranchOfficeId { get; init; }
        public int PeriodId { get; init; }
        public DateTime LiabilityDate { get; init; }
        public decimal LiabilitySum { get; init; }
        public decimal EnergyAmount { get; init; }
        public decimal TaxSum { get; init; }
        public DateTime CreationDate { get; init; }
        public decimal FullSum { get; init; }
        public int Type { get; init; }

        public IEnumerable<TaxInvoiceTabLine> TabLines { get; init; }
        /*public int RowNumber { get; init; }
        public string ProductName { get; init; }
        public string ProductCode { get; init; }
        public string Unit { get; init; }
        public string UnitCode { get; init; }
        public decimal Quantity { get; init; }
        public decimal Price { get; init; }
        public decimal LiabilitiesAmount { get; init; }
        public decimal Tax { get; init; }*/
    }
}
