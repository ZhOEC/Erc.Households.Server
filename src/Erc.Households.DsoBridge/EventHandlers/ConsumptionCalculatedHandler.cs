using Erc.Households.DsoBridge.Bus;
using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.DsoBridge.EventHandlers
{
    public class ConsumptionCalculatedHandler : IConsumer<Batch<Ztoe.Shared.DsoEvents.Households.ConsumptionCalculated>>
    {
        readonly ISendEndpoint _sendEndpoint;

        public ConsumptionCalculatedHandler(IErcBus bus) => _sendEndpoint = bus.GetSendEndpoint(new Uri("exchange:Erc.Households.Commands:CalculateAccountingPoint")).Result;

        public async Task Consume(ConsumeContext<Batch<Ztoe.Shared.DsoEvents.Households.ConsumptionCalculated>> context) =>
            await Task.WhenAll(context.Message.Select(m => _sendEndpoint.Send<Commands.CalculateAccountingPoint>(m.Message)).ToList());
    }
}
