using Erc.Households.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Shared.Addresses
{
    public class Street
    {
        City _city;

        private Action<object, string> LazyLoader { get; set; }

        protected Street(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City 
        {
            get => LazyLoader.Load(this, ref _city);
            private set { _city = value; }
        }
    }
}
