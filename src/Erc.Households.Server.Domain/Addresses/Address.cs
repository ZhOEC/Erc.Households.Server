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
    }

}
