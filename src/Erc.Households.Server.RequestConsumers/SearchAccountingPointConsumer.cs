using Erc.Households.Backend.DataAccess.PostgreSql;
using Erc.Households.Backend.Requests;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Erc.Households.Backend.RequestConsumers
{
    public class SearchAccountingPointConsumer : IConsumer<SearchAccountingPoint>
    {
        readonly ErcContext _ercContext;

        public SearchAccountingPointConsumer(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public Task Consume(ConsumeContext<SearchAccountingPoint> context)
        {
            throw new NotImplementedException();
        }
    }
}
