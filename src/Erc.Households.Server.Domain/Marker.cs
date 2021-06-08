using Erc.Households.Domain.AccountingPoints;

namespace Erc.Households.Domain
{
    public class Marker
    {
        public int Id { get; init; }
        public MarkerType Type { get; init; }
        public string Value { get; init; }
    }
}
