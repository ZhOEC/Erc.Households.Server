using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.CalculateStrategies.ElectricPower;
using Erc.Households.CalculateStrategies.NaturalGas;
using Erc.Households.EF.PostgreSQL;
using GreenPipes;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Erc.Households.Calculation
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
                          
                            cfg.UseConcurrencyLimit(1);
                            cfg.ConfigureEndpoints(ctx);
                        });
                    });
                    
                    services.AddDbContext<ErcContext>(options =>
                       options.UseNpgsql(hostContext.Configuration.GetConnectionString("ErcContext")));

                    services.AddTransient<GasCalculateStrategy>();
                    services.AddTransient(sp => new ElectricPowerCalculateStrategy(async (accountingPointId, fromDate) =>
                    {
                        var ctx = sp.GetRequiredService<ErcContext>();
                        return (await ctx.Invoices.Where(i => i.AccountingPointId == accountingPointId && i.FromDate == fromDate)
                        .Select(i => new { i.UsageT1, i.UsageT2, i.UsageT3 })
                        .ToArrayAsync())
                        .Select(u => (u.UsageT1, u.UsageT2, u.UsageT3));
                    }));

                    services.AddHostedService<CalculationService>();
                });
    }
}
