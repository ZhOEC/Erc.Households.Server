using Erc.Households.Domain;
using MediatR;
using X.PagedList;

namespace Erc.Households.Api.Queries
{
    public class GetMarkersByPart : IRequest<IPagedList<Marker>>
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }

        public GetMarkersByPart(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
