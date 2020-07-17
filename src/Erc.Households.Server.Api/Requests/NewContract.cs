using System;

namespace Erc.Households.Api.Requests
{
    public class NewContract
    {
        public DateTime ContractStartDate { get; set; }
        public bool SendPaperBill { get; set; }
        public Domain.Person Owner { get; set; }
    }
}
