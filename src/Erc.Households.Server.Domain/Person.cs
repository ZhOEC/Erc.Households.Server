using Erc.Households.Server.Domain.Addresses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string TaxCode { get; set; }
        public string IdCardNumber { get; set; }
        public DateTime IdCardIssuanceDate { get; set; }
        public DateTime? IdCardExpDate { get; set; }
        public int? AddressId { get; set; }
        public string[] MobilePhones { get; set; }
        public Address Address { get; set; }
    }
}
