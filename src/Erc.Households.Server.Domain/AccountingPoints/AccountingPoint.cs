using Erc.Households.Server.Domain.Addresses;
using Erc.Households.Server.Domain.Tariffs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.AccountingPoints
{
    public class AccountingPoint
    {
        public int Id { get; set; }
        public string Name {get;set;}
        public string Eic {get;set;}
        public int AddressId { get; set; }
        public int OwnerId { get; set; }
        public int TariffId { get; set; }
        public int DistributionSystemOperatorId { get; set; }
        public int BranchOfficeId { get; set; }
        public decimal Debt { get; set; }
        public DistributionSystemOperator Dso { get; set; }
        public Tariff Tariff { get; set; }
        public Address Address { get; set; }
        public Person Owner { get; set; }
        public BranchOffice BranchOffice { get; set; }
    }
}
