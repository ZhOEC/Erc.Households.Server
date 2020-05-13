using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.WebApi.Reponses
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string TaxCode { get; set; }
        public string IdCardNumber { get; set; }
        public DateTime IdCardIssuanceDate { get; set; }
        public DateTime? IdCardExpDate { get; set; }
        public int? AddressId { get; set; }
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
        public string[] MobilePhones { get; set; }
    }
}
