using MediatR;

namespace Erc.Households.Commands.Markers
{
    public class DeleteMarkerAccountingPointCommand : IRequest
    {
        public int AccountingPointId { get; init; }
        public int MarkerId { get; init; }

        public DeleteMarkerAccountingPointCommand(int accountingPointId, int markerId)
        {
            AccountingPointId = accountingPointId;
            MarkerId = markerId;
        }
    }
}
