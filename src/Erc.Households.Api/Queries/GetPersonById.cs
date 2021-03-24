using MediatR;

namespace Erc.Households.Api.Queries
{
    public class GetPersonById : IRequest<Domain.Shared.Person> 
    {
        public GetPersonById(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
