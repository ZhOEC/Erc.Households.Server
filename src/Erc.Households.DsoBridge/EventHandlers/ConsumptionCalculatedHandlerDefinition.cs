using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.DsoBridge.EventHandlers
{
    public class ConsumptionCalculatedHandlerDefinition : ConsumerDefinition<ConsumptionCalculatedHandler>
    {
        public ConsumptionCalculatedHandlerDefinition() => Endpoint(x => x.PrefetchCount = 100);
    }
}
