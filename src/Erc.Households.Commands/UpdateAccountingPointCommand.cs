using MediatR;

namespace Erc.Households.Commands
{
    public class UpdateAccountingPointCommand : IRequest<Unit>
    {
        public UpdateAccountingPointCommand(int id, string eic, string name, int branchOfficeId, int distributionSystemOperatorId,
                                            int streetId, string building, string apt, string zip, int buildingTypeId, int usageCategoryId)
        {
            Id = id;
            Eic = eic;
            Name = name;
            BranchOfficeId = branchOfficeId;
            DistributionSystemOperatorId = distributionSystemOperatorId;
            StreetId = streetId;
            Building = building;
            Apt = apt;
            Zip = zip;
            BuildingTypeId = buildingTypeId;
            UsageCategoryId = usageCategoryId;

        }

        public int Id { get; private set; }
        public string Eic { get; private set; }
        public string Name { get; private set; }
        public int BranchOfficeId { get; private set; }
        public int DistributionSystemOperatorId { get; private set; }
        public int ZoneRecord { get; private set; }
        public int StreetId { get; private set; } 
        public string Building { get; private set; } 
        public string Apt { get; private set; }
        public string Zip { get; private set; }
        public int BuildingTypeId { get; private set; }
        public int UsageCategoryId { get; private set; }
    }
}
