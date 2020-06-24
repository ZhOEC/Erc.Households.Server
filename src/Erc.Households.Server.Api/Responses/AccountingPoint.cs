using System;

namespace Erc.Households.Api.Responses
{
    public class AccountingPoint
    {
        public int Id { get; set; }
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
        public decimal Debt { get; set; }
        public AccountingPointExemption Exemption { get; set; }
    }

    public class AccountingPointExemption
    {
        public DateTime EffectiveDate { get; private set; }
        public int PersonsNumber { get; private set; }
        public string CategoryName { get; private set; }
        public Person Person { get; private set; }
    }
}
