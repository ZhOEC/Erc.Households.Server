namespace Erc.Households.Server.Domain.Events
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccountingPointPersisted
    {
        int Id { get; }
        string BranchOfficeStringId { get; }
        string Name { get; }
        string Eic { get; }
        string PersonFirstName { get; }
        string PersonLastName { get; }
        string PersonPatronymic { get; }
        string PersonTaxCode { get; }
        string PersonIdCardNumber { get; }
        string Address { get; }
    }
}
