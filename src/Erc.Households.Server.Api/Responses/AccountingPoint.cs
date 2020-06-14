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
    }
}
