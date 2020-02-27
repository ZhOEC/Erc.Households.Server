using System;

namespace Erc.Households.Backend.Responses
{
    public class AccountingPointListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Eic { get; set; }
        public string Address { get; set;}
        public string Owner { get; set;}
        public string TariffName { get; set;}
        public string BranchOfficeName { get; set;}
    }
}
