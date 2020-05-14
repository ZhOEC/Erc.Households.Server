using System;
using Erc.Households.Domain.Addresses;

namespace Erc.Households.Api.Requests
{ 
    public class NewAccountingPoint
    {
        public string Eic { get; set; }
        public string Name { get; set; }
        public DateTime ContractStartDate { get; set; }
        public int TariffId { get; set; }
        public Address Address { get; set; }
        public Domain.Person Owner { get; set; }
        public int BranchOfficeId { get; set; }
        public int DsoId { get; set; }
        public Domain.AccountingPoints.ZoneRecord ZoneRecord { get; set; } = Domain.AccountingPoints.ZoneRecord.None;
    }
}