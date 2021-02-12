using Erc.Households.PrintBills.Api.Helpers;
using System;
using System.Collections.Generic;

namespace Erc.Households.PrintBills.Api.Models
{
    public class BillElectricity
    {
        public int AccountinPointId { get; set; }
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanySite { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyStateRegistryCode { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchOfficeIban { get; set; }
        public string BranchOfficeBankFullName { get; set; }
        public string AccountingPointName { get; set; }
        public string Eic { get; set; }
        public string AccountingPointAddress { get; set; }
        public string OwnerFullName { get; set; }
        public string PeriodShortDate { get; set; }
        public string ContractStartDate { get; set; }
        public string PeriodName { get; set; }
        public decimal? AccountingPointDebtHistory { get; set; }
        public decimal? PaymentsSumByPeriod { get; set; }
        public decimal? CompensationSumByPeriod { get; set; }
        public decimal? InvoiceTotalUnits { get; set; }
        public decimal InvoiceTotalAmountDue { get; set; }
        public int DiscountUnitsSum { get; set; }
        public decimal? DiscountSum { get; set; }
        public string Barcode => $"*1+{AccountinPointId.To36()}+{Convert.ToInt32(Math.Floor(InvoiceTotalAmountDue)).To36()}+{Convert.ToInt32((InvoiceTotalAmountDue - Convert.ToInt32(Math.Floor(InvoiceTotalAmountDue))) * 100).To36()}*";
        public Usage UsageT1 { get; set; }
        public Usage UsageT2 { get; set; }
        public Usage UsageT3 { get; set; }
    }

    public class Usage
    {
        public int Units { get; set; }
        public decimal? Charge { get; set; }
        public int PresentMeterReading { get; set; }
        public int PreviousMeterReading { get; set; }
        public int DiscountUnits { get; set; }
        public List<Calculation> Calculations { get; set; }
    }

    public class Calculation
    {
        public int Units { get; set; }
        public decimal? Charge { get; set; }
        public decimal? PriceValue { get; set; }
        public int DiscountUnits { get; set; }
        public decimal? Discount { get; set; }

    }
}
