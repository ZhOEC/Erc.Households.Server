using Erc.Households.Domain.Shared;
using System;

namespace Erc.Households.Api.Requests
{
    public record NewAccountingPoint
    {
        public string Eic { get; set; }
        public string Name { get; set; }
        public DateTime ContractStartDate { get; set; }
        public int TariffId { get; set; }
        public int BranchOfficeId { get; set; }
        public int DistributionSystemOperatorId { get; set; }
        public ZoneRecord ZoneRecord { get; set; } = ZoneRecord.None;
        public int BuildingTypeId { get; set; }
        public int UsageCategoryId { get; set; }
        public bool SendPaperBill { get; set; }
        public Domain.Shared.Addresses.Address Address { get; set; }
        public Person Owner { get; set; }
        public Commodity Commodity { get; set; }
        public bool? IsGasWaterHeaterInstalled { get; set; }
        public bool? IsCentralizedWaterSupply { get; set; }
        public bool? IsCentralizedHotWaterSupply { get; set; }
    }
}