using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zt.Energy.Dso;

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
                    services.AddMassTransit(s =>
                    {
                        s.AddConsumers(System.Reflection.Assembly.GetExecutingAssembly());
                        s.UsingRabbitMq((ctx, cfg) =>
                        {
                            var rabbitMq = hostContext.Configuration.GetSection("RabbitMQ");
                            cfg.Host(rabbitMq["ConnectionString"], c =>
                            {
                                c.Username(rabbitMq["Username"]);
                                c.Password(rabbitMq["Password"]);
                            });
                            cfg.UseConcurrencyLimit(int.Parse(rabbitMq["ConcurrencyLimit"] ?? "8"));
                            cfg.ConfigureEndpoints(ctx);
                        });
                    });

                    services.AddMassTransit<IZtoeBus>(s =>
                    {
                        s.AddConsumers(System.Reflection.Assembly.GetExecutingAssembly());
                        s.UsingRabbitMq((ctx, cfg) =>
                        {
                            var rabbitMq = hostContext.Configuration.GetSection("ZtoeRabbitMQ");
                            cfg.Host(rabbitMq["ConnectionString"], c =>
                            {
                                c.Username(rabbitMq["Username"]);
                                c.Password(rabbitMq["Password"]);
                            });
                            cfg.UseConcurrencyLimit(int.Parse(rabbitMq["ConcurrencyLimit"] ?? "8"));
                            cfg.ConfigureEndpoints(ctx);
                        });
                    });

                    services.AddHostedService<BridgeService>();
                });
    }
}
