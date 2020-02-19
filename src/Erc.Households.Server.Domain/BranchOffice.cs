using System.Linq;

namespace Erc.Households.Server.Domain
{
    public class BranchOffice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StringId { get; set; }
        public int[] DistrictIds { get; private set; }
        public string Address { get; set; }

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
