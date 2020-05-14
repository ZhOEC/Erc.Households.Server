using Erc.Households.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Domain.Addresses
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

        public string StreetLocation => $"{Street.Name} {Building}{(string.IsNullOrEmpty(Apt) ? string.Empty : ", кв. " + Apt)}";
        public string CityName => Street.City.ToString();
        public override string ToString() => $"{StreetLocation}, {Street.City}";
    }

}
