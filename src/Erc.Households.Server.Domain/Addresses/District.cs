using Erc.Households.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Addresses
{
    public class District
    {
        Region _region;
        private Action<object, string> LazyLoader { get; set; }

        protected District(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public Region Region 
        {
            get => LazyLoader.Load(this, ref _region);
            private set { _region = value; }
        }
}
}
