using Erc.Households.Backend.Data.Addresses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Backend.Data
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string TaxCode { get; set; }
        public string IdCardNumber { get; set; }
        public DateTime? IdCardExpDate { get; set; }
        public int? AddressId { get; set; }
        public string MobilePhone1 { get; set; }
        public string MobilePhone2 { get; set; }
        public Address Address { get; set; }
    }
}
