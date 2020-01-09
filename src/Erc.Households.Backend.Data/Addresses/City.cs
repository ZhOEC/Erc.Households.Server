using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Backend.Data.Addresses
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }
        public bool IsDistrictCity { get; set; }
        public bool IsRegionCity { get; set; }
    }
}
