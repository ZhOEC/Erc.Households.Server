using Erc.Households.Server.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Addresses
{
    public class Address
    {
        Street _street;

        private Action<object, string> LazyLoader { get; set; }

        public Address()
        {

        }

        protected Address(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public int Id { get; set; }
        public string Zip { get; set; }
        public int StreetId { get; set; }
        public string Building { get; set; }
        public string Apt { get; set; }
        public Street Street 
        {
            get => LazyLoader.Load(this, ref _street);
            private set { _street = value; }
        }
        public override string ToString() => 
            $"{Street.Name}, {Building}{(string.IsNullOrEmpty(Apt) ? string.Empty : ", кв. " + Apt)}, {Street.City.Name}, {(Street.City.IsRegionCity ? string.Empty : Street.City.District.Name)}";
    }

}
