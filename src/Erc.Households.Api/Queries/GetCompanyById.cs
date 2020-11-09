using MediatR;

namespace Erc.Households.Api.Queries
{
    public class GetCompanyById : IRequest<Domain.Company>
    {
        public GetCompanyById(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
