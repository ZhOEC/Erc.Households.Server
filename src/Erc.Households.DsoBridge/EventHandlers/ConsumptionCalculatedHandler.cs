using MassTransit;
using System.Threading.Tasks;

namespace Erc.Households.DsoBridge.EventHandlers
{
    public class ConsumptionCalculatedHandler : IConsumer<Zt.Energy.Dso.Events.Households.ConsumptionCalculated>
    {
        readonly IErcBus _ercBus;

        public ConsumptionCalculatedHandler(IErcBus bus) => _ercBus = bus;

        public async Task Consume(ConsumeContext<Zt.Energy.Dso.Events.Households.ConsumptionCalculated> context) => await _ercBus.Publish<Events.AccountingPoints.ConsumptionCalculated>(context.Message);
    }
}
