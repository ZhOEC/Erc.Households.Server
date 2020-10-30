using Erc.Households.Domain.Addresses;
using Erc.Households.Domain.Shared;
using System;

namespace Erc.Households.Api.Responses
{
    public class AccountingPoint
    {
        public int Id { get; set; }
        public int BranchOfficeId { get; private set; }
        public int DistributionSystemOperatorId { get; private set; }
        public int TariffId { get; private set; }
        public int UsageCategoryId { get; private set; }
        public int BuildingTypeId { get; private set; }
        public City City { get; private set; }
        public Address Address { get; private set; }
        public DateTime ContractStartDate { get; private set; }
        public bool SendPaperBill { get; private set; }

        public string BranchOfficeName { get; set; }
        public string Name { get; set; }
        public string DistributionSystemOperatorName { get; set; }
        public string CurrentTariffName { get; set; }
        public string AddressCityName { get; set; }
        public string AddressZip { get; set; }
        public string AddressStreetLocation { get; set; }
        public Person Owner { get; set; }
        public string Eic { get; set; }
        public DateTime CurrentContractStartDate { get; set; }
        public bool CurrentContractSendPaperBill { get; set; }
        public decimal Debt { get; set; }
        public AccountingPointExemption Exemption { get; set; }
        public Commodity Commodity { get; private set; }
    }

    public class AccountingPointExemption
    {
        public DateTime EffectiveDate { get; private set; }
        public int PersonsNumber { get; private set; }
        public string CategoryName { get; private set; }
        public Person Person { get; private set; }
    }
}
