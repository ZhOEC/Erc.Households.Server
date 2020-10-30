using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.DsoBridge.EventHandlers
{
    public class ConsumptionCalculatedHandler : IConsumer<Batch<Ztoe.Shared.DsoEvents.Households.ConsumptionCalculated>>
    {
        readonly ISendEndpoint _sendEndpoint;

        public ConsumptionCalculatedHandler(ISendEndpoint sendEndpoint) => _sendEndpoint = sendEndpoint;

        public async Task Consume(ConsumeContext<Batch<Ztoe.Shared.DsoEvents.Households.ConsumptionCalculated>> context) =>
            await Task.WhenAll(context.Message.Select(m => _sendEndpoint.Send<Commands.CalculateAccountingPoint>(m.Message)).ToList());
    }
}
