﻿using System;

namespace Erc.Households.Api.Responses
{
    public class Person
    {
        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string TaxCode { get; set; }
        public string IdCardNumber { get; set; }
        public string IdCardIssuer { get; set; }
        public DateTime IdCardIssuanceDate { get; set; }
        public DateTime? IdCardExpDate { get; set; }
        public int? AddressId { get; set; }
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
        public string[] MobilePhones { get; set; }
        public string Email { get; set; }
    }
}
