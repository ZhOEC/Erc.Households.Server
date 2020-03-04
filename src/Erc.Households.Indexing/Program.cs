using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Indexing.EventHandlers;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;

namespace Erc.Households.Indexing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables().AddJsonFile("appsettings.json");
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IElasticClient>(provider => new ElasticClient(new Uri(hostContext.Configuration.GetConnectionString("Elasticsearch"))));

                services.AddMassTransit(x =>
                {
                    x.AddConsumer<AccountingPointPersistedEventHandler>();

                    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(hostContext.Configuration.GetConnectionString("RabbitMQ"));
                        cfg.ConfigureEndpoints(provider);
                    }));
                        
                    services.AddHostedService<IndexongService>();
                });
            });
    }
}
