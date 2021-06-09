namespace Erc.Households.Domain.AccountingPoints
{
    public class AccountingPointMarker
    {
        public int Id { get; init; }
        public int AccountingPointId { get; init; }
        public int MarkerId { get; init; }
        public string Note { get; init; }
        public Marker Marker { get; init; }

        public AccountingPointMarker(int accountingPointId, int markerId, string note)
        {
            AccountingPointId = accountingPointId;
            MarkerId = markerId;
            Note = note;
        }
    }
}
