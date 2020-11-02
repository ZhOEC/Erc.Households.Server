using MediatR;

namespace Erc.Households.Api.Queries.AccountingPoints
{
    public class GetAccountingPointById : IRequest<Responses.AccountingPoint>
    {
        public int Id { get; private set; }
        public string Eic { get; private set; }

        public GetAccountingPointById(string id)
        {
            if (int.TryParse(id, out int intId)) Id = intId;
            else Eic = id;
        }
    }
}
