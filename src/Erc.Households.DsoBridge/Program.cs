using Erc.Households.DsoBridge.Bus;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Erc.Households.DsoBridge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit<IErcBus>(s =>
                    {
                        s.UsingRabbitMq((ctx, cfg) =>
                        {
                            var rabbitMq = hostContext.Configuration.GetSection("RabbitMQ");
                            cfg.Host(rabbitMq["ConnectionString"], c =>
                            {
                                c.Username(rabbitMq["Username"]);
                                c.Password(rabbitMq["Password"]);
                            });
                        });
                    });

                    services.AddMassTransit(s =>
                    {
                        var rabbitMq = hostContext.Configuration.GetSection("ZtoeRabbitMQ");
                        s.UsingRabbitMq((ctx, cfg) =>
                        {
                            cfg.Host(rabbitMq["ConnectionString"], c =>
                            {
                                c.Username(rabbitMq["Username"]);
                                c.Password(rabbitMq["Password"]);
                            });
                            cfg.ReceiveEndpoint("consumption-calculated-ztoec", e =>
                            {
                                e.PrefetchCount = 500;
                                e.Batch<Ztoe.Shared.DsoEvents.Households.ConsumptionCalculated>( b =>
                                {
                                    b.MessageLimit = 100;
                                    b.ConcurrencyLimit = 10;

                                    var sendEndpoint =  services.BuildServiceProvider().GetRequiredService<IErcBus>().GetSendEndpoint(new Uri("exchange:Erc.Households.Commands:CalculateAccountingPoint")).Result;
                                    b.Consumer(() => new EventHandlers.ConsumptionCalculatedHandler(sendEndpoint));
                                });
                            });
                        });
                    });

                    services.AddHostedService<BridgeService>();
                });
    }
}
