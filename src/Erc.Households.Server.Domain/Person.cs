using Erc.Households.Domain.Addresses;
using System;

namespace Erc.Households.Domain
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
        public string IdCardIssuer { get; set; }
        public DateTime? IdCardExpDate { get; set; }
        public int? AddressId { get; set; }
        public string[] MobilePhones { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }
}
