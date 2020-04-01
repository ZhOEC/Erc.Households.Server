using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Addresses
{
    public class Address
    {
        public int Id { get; set; }
        public string Zip { get; set; }
        public int StreetId { get; set; }
        public string Building { get; set; }
        public string Apt { get; set; }
        public Street Street { get; set; }
        public override string ToString() => 
            $"{(Street.City.IsRegionCity ? string.Empty : Street.City.District.Name)} {Street.City.Name} {Street.Name} {Building}{(string.IsNullOrEmpty(Apt) ? string.Empty : "кв. " + Apt)}";
    }

}
