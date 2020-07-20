using Erc.Households.Api.Responses;

namespace Erc.Households.Api.Requests
{
    public class UpdatedAccountingPoint
    {
        public int Id { get; set; }
        public string Eic { get; set; }
        public string Name { get; set; }
        public int BranchOfficeId { get; set; }
        public int DistributionSystemOperatorId { get; set; }
        public Address Address { get; set; }
        public int BuildingTypeId { get; set; }
        public int UsageCategoryId { get; set; }
    }
}
