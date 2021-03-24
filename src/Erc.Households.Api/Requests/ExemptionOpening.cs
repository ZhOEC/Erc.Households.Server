using Erc.Households.Domain.Shared;
using System;

namespace Erc.Households.Api.Requests
{
    public class ExemptionOpening
    {
        public int ExemptionCategoryId { get; set; }
        public DateTime Date { get; set; }
        public string Certificate { get; set; }
        public int PersonCount { get; set; }
        public bool Limit { get; set; }
        public string Note { get; set; }
        public Person Person { get; set; }
    }
}
