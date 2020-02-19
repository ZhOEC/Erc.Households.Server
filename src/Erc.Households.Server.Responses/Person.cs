using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Backend.Responses
{
    public interface Person
    {
        int Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string Patronymic { get; }
        string TaxCode { get; }
        string IdCardNumber { get; }
        DateTime? IdCardExpDate { get; }
        string MobilePhone1 { get; }
        string MobilePhone2 { get; }
    }
}
