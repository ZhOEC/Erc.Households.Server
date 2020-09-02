using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.DsoBridge.EventHandlers
{
    public class ConsumptionCalculatedHandlerDefinition: ConsumerDefinition<ConsumptionCalculatedHandler>
    {
        public ConsumptionCalculatedHandlerDefinition() => Endpoint(x => x.PrefetchCount = 1000);

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<ConsumptionCalculatedHandler> consumerConfigurator)
        {
            consumerConfigurator.Options<BatchOptions>(options => options
                .SetMessageLimit(100)
                .SetTimeLimit(1000)
                .SetConcurrencyLimit(10));
        }
    }
}
