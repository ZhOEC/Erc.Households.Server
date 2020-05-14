namespace Erc.Households.Events.AccountingPoints
{
    /// <summary>
    /// Event rises after accounting point persisted in database.
    /// </summary>
    public class AccountingPointCreated: IEntityEvent
    {
        public int Id { get; set; }
        public string BranchOfficeStringId { get; set;}
        public string BranchOfficeName { get; set;}
        public string Name { get; set;}
        public string Eic { get; set;}
        public string PersonFirstName { get; set;}
        public string PersonLastName { get; set;}
        public string PersonPatronymic { get; set;}
        public string PersonTaxCode { get; set;}
        public string PersonIdCardNumber { get; set;}
        public string CityName { get; set;}
        public string StreetAddress { get;  set;}
    }
}
