using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Shared;
using System.Linq;

namespace Erc.Households.Domain
{
    public class BranchOffice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StringId { get; set; }
        public int[] DistrictIds { get; private set; }
        public string Address { get; private set; }
        public Commodity[] AvailableCommodities { get; set; }
        public int CurrentPeriodId { get; private set; }
        public int? CompanyId { get; set; }
        public string AccountNumber { get; set; }
        public string ChiefName { get; set; }
        public string BookkeeperName { get; set; }
        public Period CurrentPeriod { get; private set; }
        public Company Company { get; private set; }

        public void StartNewPeriod(Period period)
        {
            CurrentPeriod = period;
        }

        public void AddDistrict(int districtId)
        {
            if (!DistrictIds.Contains(districtId))
            {
                var a = new int[DistrictIds.Length + 1];
                DistrictIds.CopyTo(a, 0);
                a[DistrictIds.Length] = districtId;
                DistrictIds = a;
            }
        }

        public void RemoveDistrict(int districtId)
        {
            if (DistrictIds.Contains(districtId))
            {
                var a = new int[DistrictIds.Length - 1];
                int index = 0;
                foreach (var d in DistrictIds)
                    if (d != districtId)
                        a[index++] = d;
                DistrictIds = a;
            }
        }
    }
}
