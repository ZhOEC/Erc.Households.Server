using Erc.Households.Api.Responses;

namespace Erc.Households.Api.Requests
{
    public record UpdatedAccountingPoint
    {
        public int Id { get; init; }
        public string Eic { get; init; }
        public string Name { get; init; }
        public int BranchOfficeId { get; init; }
        public int DistributionSystemOperatorId { get; init; }
        public Address Address { get; init; }
        public int BuildingTypeId { get; init; }
        public int UsageCategoryId { get; init; }
        public bool? IsGasWaterHeaterInstalled { get; init; }
        public bool? IsCentralizedWaterSupply { get; init; }
        public bool? IsCentralizedHotWaterSupply { get; init; }
    }
}
