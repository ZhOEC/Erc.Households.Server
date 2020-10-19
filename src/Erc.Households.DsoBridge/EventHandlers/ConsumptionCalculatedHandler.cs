using Erc.Households.DsoBridge.Bus;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.DsoBridge.EventHandlers
{
    public class ConsumptionCalculatedHandler : IConsumer<Batch<Ztoe.Shared.DsoEvents.Households.ConsumptionCalculated>>
    {
        readonly IErcBus _ercBus;

        public ConsumptionCalculatedHandler(IErcBus bus) => _ercBus = bus;

        public async Task Consume(ConsumeContext<Batch<Ztoe.Shared.DsoEvents.Households.ConsumptionCalculated>> context) => 
            await Task.WhenAll(context.Message.Select(m => _ercBus.Send<Commands.CalculateAccountingPoint> (m.Message)).ToList());
    }
}
