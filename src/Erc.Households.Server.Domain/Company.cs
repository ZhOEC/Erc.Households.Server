using System.Collections.Generic;

namespace Erc.Households.Domain
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string DirectorName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Www { get; set; }
        public string TaxpayerPhone { get; set; }
        public string StateRegistryCode { get; set; }
        public string TaxpayerNumber { get; set; }
        public string BookkeeperName { get; set; }
        public string BookkeeperTaxNumber { get; set; }
        public ICollection<BranchOffice> BranchOffice { get; private set; }
    }
}
