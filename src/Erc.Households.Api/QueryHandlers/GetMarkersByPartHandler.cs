using Erc.Households.Domain;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace Erc.Households.Api.Queries
{
    public class GetMarkersByPartHandler : IRequestHandler<GetMarkersByPart, IPagedList<Marker>>
    {
        private readonly ErcContext _ercContext;

        public GetMarkersByPartHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<IPagedList<Marker>> Handle(GetMarkersByPart request, CancellationToken cancellationToken)
            => await _ercContext.Markers.OrderByDescending(x => x.Id).ToPagedListAsync(request.PageNumber, request.PageSize);
    }
}
