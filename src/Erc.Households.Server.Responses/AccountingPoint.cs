using System;

namespace Erc.Households.Backend.Responses
{
    public interface AccountingPoint
    {
        int Id { get; }
        string Name { get; }
        string Eic { get; }
        string Address { get; }
        Person Owner { get; }
        string TariffName { get; }
        string DsoName { get; }
        int BranchOfficeName { get; }
    }
}
