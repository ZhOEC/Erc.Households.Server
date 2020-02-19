using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Addresses
{
    public class Region
    {
        readonly ICollection<District> _districts = new HashSet<District>();

        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<District> Districts => _districts;
    }
}
