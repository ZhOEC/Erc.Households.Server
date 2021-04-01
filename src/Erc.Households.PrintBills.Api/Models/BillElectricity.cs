using System;
using System.Collections.Generic;
using System.Linq;
using Erc.Households.PrintBills.Api.Helpers;

namespace Erc.Households.PrintBills.Api.Models
{
    public class BillElectricity
    {
        public int AccountinPointId { get; set; }
        public string AccountingPointName { get; set; }
        public string OwnerFullName { get; set; }
        public int Zip { get; set; }
        public string City { get; set; }
        public string CompanyFullName { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanySite { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyStateRegistryCode { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchOfficeName { get; set; }
        public string BranchOfficeAddress { get; set; }
        public string BranchOfficeIban { get; set; }
        public string BranchOfficeBankFullName { get; set; }
        public int BranchOfficePrivatBankId { get; set; }
        public string Eic { get; set; }
        public string AccountingPointAddress { get; set; }
        public string PeriodName { get; set; }
        public string PeriodShortDate { get; set; }
        public string ContractStartDate { get; set; }
        public decimal? AccountingPointDebtHistory { get; set; }
        public decimal? ExemptionCoeff { get; set; }
        public decimal? PaymentsSumByPeriod { get; set; }
        public decimal? CompensationSumByPeriod { get; set; }
        public decimal? InvoiceTotalUnits { get; set; }
        public decimal InvoiceTotalAmountDue { get; set; }
        public int DiscountUnitsSum { get; set; }
        public decimal? DiscountSum { get; set; }
        public string QrPrivatBank => $"EK_V3_ls_{AccountingPointName}_{BranchOfficePrivatBankId}";
        public string Barcode => $"*1+{AccountinPointId.To36()}+{Convert.ToInt32(Math.Floor(InvoiceTotalAmountDue)).To36()}+{Convert.ToInt32((InvoiceTotalAmountDue - Convert.ToInt32(Math.Floor(InvoiceTotalAmountDue))) * 100).To36()}*";
        public int ZoneCount {
            get {
                if (UsageT2 != null && UsageT3 is null)
                {
                    return 2;
                } else if (UsageT3 != null) {
                    return 3;
                }

                return 1;
            }
        }
        public Usage UsageT1 { get; set; }
        public Usage UsageT2 { get; set; }
        public Usage UsageT3 { get; set; }
    }

    public class Usage
    {
        public decimal? Kz { get; set; }
        public decimal? Units { get; set; }
        public decimal? Charge { get; set; }
        public int? PresentMeterReading { get; set; }
        public int? PreviousMeterReading { get; set; }
        public decimal? Discount { get; set; }
        public int? DiscountUnits { get; set; }
        public List<Calculation> Calculations { get; set; }
        public List<Calculation> GroupCalculations {
            get
            {
                return Calculations.GroupBy(x => x.PriceValue)
                    .Select(x => new Calculation()
                    {
                        PriceValue = x.Key,
                        Charge = x.Sum(y => y.Charge),
                        Units = x.Sum(y => y.Units),
                        Discount = x.Sum(y => y.Discount),
                        DiscountUnits = x.Sum(y => y.DiscountUnits),
                    })
                    .OrderBy(x => x.PriceValue)
                    .ToList();
            }
        }
    }

    public class Calculation
    {
        public decimal? PriceValue { get; set; }
        public int? Units { get; set; }
        public decimal? Charge { get; set; }
        public int? DiscountUnits { get; set; }
        public decimal? Discount { get; set; }

    }
}
