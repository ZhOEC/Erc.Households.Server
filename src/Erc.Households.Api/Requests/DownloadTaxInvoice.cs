using Erc.Households.Domain.Taxes;
using System;

namespace Erc.Households.Api.Requests
{
    public class DownloadTaxInvoice
    {
        public int Id { get; set; }
        public TaxInvoiceType Type { get; set; }
        public DateTime LiabilityDate { get; set; }
        public decimal LiabilitySum { get; set; }
        public decimal EnergyAmount { get; set; }
        public decimal TariffValue { get; set; }
        public decimal TaxSum { get; set; }
        public decimal FullSum { get; set; }
        public DateTime CreationDate { get; set; }

        public int BranchOfficeId { get; set; }
        public string BranchOfficeName { get; set; }

        public string CompanyAddress { get; set; }
        public string CompanyTaxpayerPhone { get; set; }
        public string CompanyStateRegistryCode { get; set; }
        public string CompanyTaxpayerNumber { get; set; }
        public string CompanyBookkeeperName { get; set; }
        public string CompanyBookkeeperTaxNumber { get; set; }
    }
}
