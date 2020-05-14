﻿using Erc.Households.Events;

namespace Erc.Households.Events.AccountingPoints
{
    /// <summary>
    /// Event rised after accouting point persisted in database.
    /// </summary>
    public class AccountingPointUpdated: IEntityEvent
    {
        public int Id { get; set;  }
        public string BranchOfficeStringId { get; set;}
        public string Name { get; set;}
        public string Eic { get; set;}
        public string PersonFirstName { get; set;}
        public string PersonLastName { get; set;}
        public string PersonPatronymic { get; set;}
        public string PersonTaxCode { get; set;}
        public string PersonIdCardNumber { get; set;}
        public string Address { get; set;}
    }
}
