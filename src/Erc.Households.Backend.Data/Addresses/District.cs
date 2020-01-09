using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Backend.Data.Addresses
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
}
}
