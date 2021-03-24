using Erc.Households.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Shared.Addresses
{
    public class City
    {
        District _district;

        private Action<object, string> LazyLoader { get; set; }

        protected City(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int DistrictId { get; set; }
        public District District 
        {
            get => LazyLoader.Load(this, ref _district);
            private set { _district = value; }
        }
        public bool IsDistrictCity { get; set; }
        public bool IsRegionCity { get; set; }

        public override string ToString() => $"{Name}{((IsRegionCity || IsDistrictCity) ? string.Empty : ", " + District.Name)}";
    }
}
