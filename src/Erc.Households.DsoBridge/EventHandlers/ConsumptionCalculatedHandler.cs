using Erc.Households.DsoBridge.Bus;
using MassTransit;
using System.Threading.Tasks;

namespace Erc.Households.DsoBridge.EventHandlers
{
    public class ConsumptionCalculatedHandler : IConsumer<Batch<Ztoe.Shared.DsoEvents.Households.ConsumptionCalculated>>
    {
        readonly IErcBus _ercBus;

        public ConsumptionCalculatedHandler(IErcBus bus) => _ercBus = bus;
               
        public async Task Consume(ConsumeContext<Batch<Ztoe.Shared.DsoEvents.Households.ConsumptionCalculated>> context)
        {
            for (int i = 0; i < context.Message.Length; i++)
            {
                await _ercBus.Publish<Events.AccountingPoints.ConsumptionCalculated>(context.Message[i].Message);
            }
        }
    }
}
