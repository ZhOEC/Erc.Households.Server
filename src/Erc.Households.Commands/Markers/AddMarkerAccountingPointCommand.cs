using Erc.Households.Domain.AccountingPoints;

namespace Erc.Households.Commands
{
    public class AddMarkerAccountingPointCommand : MediatR.IRequest
    {
        public int AccountingPointId { get; init; }
        public int MarkerId { get; init; }
        public string Note { get; init; }

        public AddMarkerAccountingPointCommand(int accountingPointId, int markerId, string note)
        {
            AccountingPointId = accountingPointId;
            MarkerId = markerId;
            Note = note;
        }
    }
}
