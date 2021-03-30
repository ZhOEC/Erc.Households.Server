using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.PrintBills.Api.Models
{
    public class BillNaturalGas
    {
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanySite { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyStateRegistryCode { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchOfficeIban { get; set; }
        public string BranchOfficeBankFullName { get; set; }
        public string AccountingPointName { get; set; }
        public string AccountingPointAddress { get; set; }
        public string OwnerFullName { get; set; }
        public string PeriodShortDate { get; set; }
        public decimal? AccountingPointDebtHistory { get; set; }
        public decimal? PaymentSumByPeriod { get; set; }
        public decimal? InvoiceTotalUnits { get; set; }
        public decimal? InvoiceTotalAmountDue { get; set; }
        public decimal? AccountingPointDebt { get; set; }
        public decimal TariffRate { get; set; }
    }
}
