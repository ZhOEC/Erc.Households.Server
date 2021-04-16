using MediatR;

namespace Erc.Households.Commands
{
    public record UpdateAccountingPointCommand : IRequest
    {
        public int Id { get; init; }
        public string Eic { get; init; }
        public string Name { get; init; }
        public int DistributionSystemOperatorId { get; init; }
        public int ZoneRecord { get; init; }
        public int StreetId { get; init; } 
        public string Building { get; init; } 
        public string Apt { get; init; }
        public string Zip { get; init; }
        public int BuildingTypeId { get; init; }
        public int UsageCategoryId { get; init; }
        public Domain.Shared.Addresses.Address Address { get; init; }
        public bool? IsGasWaterHeaterInstalled { get; init; }
        public bool? IsCentralizedWaterSupply { get; init; }
        public bool? IsCentralizedHotWaterSupply { get; init; }
    }
}
