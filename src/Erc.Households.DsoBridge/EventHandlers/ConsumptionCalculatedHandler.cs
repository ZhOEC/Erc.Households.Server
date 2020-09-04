using Erc.Households.DsoBridge.Bus;
using MassTransit;
using System.Threading.Tasks;

namespace Erc.Households.DsoBridge.EventHandlers
{
    public class ConsumptionCalculatedHandler : IConsumer<Batch<Events.Ztoec.ConsumptionCalculated>>
    {
        readonly IErcBus _ercBus;

        public ConsumptionCalculatedHandler(IErcBus bus) => _ercBus = bus;
               
        public async Task Consume(ConsumeContext<Batch<Events.Ztoec.ConsumptionCalculated>> context)
        {
            for (int i = 0; i < context.Message.Length; i++)
            {
                await _ercBus.Publish<Households.Events.AccountingPoints.ConsumptionCalculated>(context.Message[i].Message);
            }
        }
    }
}
