using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Shared;
using System.Collections.Generic;

namespace Erc.Households.Api.Responses
{
    public class BranchOffice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Period CurrentPeriod { get; private set; }
        public IEnumerable<Commodity> AvailableCommodities { get; private set; }
}
}
