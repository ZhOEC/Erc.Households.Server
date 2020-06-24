using MediatR;

namespace Erc.Households.Api.Queries.AccountingPoints
{
    public class GetAccountingPointById : IRequest<Responses.AccountingPoint>
    {
        public int Id { get; private set; }
        
        public GetAccountingPointById(int id)
        {
            Id = id;
        }
    }
}
