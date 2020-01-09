using Erc.Households.Backend.Data.Addresses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Backend.Data
{
    public class AccountingPoint
    {
        public int Id { get; set; }
        public string Name {get;set;}
        public string Eic {get;set;}
        public int AddressId { get; set; }
        public int PersonId { get; set; }
        public Address Address { get; set; }
        public Person Person { get; set; }
    }
}
