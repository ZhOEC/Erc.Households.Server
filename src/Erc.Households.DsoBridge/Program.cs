using Erc.Households.DsoBridge.Bus;
using GreenPipes;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                        s.AddConsumer<EventHandlers.ConsumptionCalculatedHandler>(typeof(EventHandlers.ConsumptionCalculatedHandlerDefinition)).Endpoint(cfg => cfg.Name = "consumption-calculated-ztoec");
                        s.UsingRabbitMq((ctx, cfg) =>
                        {
                            
                            cfg.Host(rabbitMq["ConnectionString"], c =>
                            {
                                c.Username(rabbitMq["Username"]);
                                c.Password(rabbitMq["Password"]);
                            });
                            //cfg.ConfigureEndpoints(ctx);
                        });
                    });

                    services.AddHostedService<BridgeService>();
                });
    }
}
