using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Erc.Households.EF.PostgreSQL;
using GreenPipes;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erc.Households.Calculation
{
    public class CalculationWorker : IHostedService
    {
        private readonly ILogger<CalculationWorker> _logger;
        readonly IConfiguration _configuration;
        IServiceProvider _serviceProvider;
        IBusControl _busControl;

        public CalculationWorker(ILogger<CalculationWorker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var services = new ServiceCollection();
            services.AddMassTransit(s => 
            {
                s.AddConsumers(Assembly.GetExecutingAssembly());
                s.UsingRabbitMq((ctx, cfg) =>
                {
                    var rabbitMq = _configuration.GetSection("RabbitMQ");
                    cfg.Host(rabbitMq["ConnectionString"], c =>
                    {
                        c.Username(rabbitMq["Username"]);
                        c.Password(rabbitMq["Password"]);
                    });
                    cfg.UseDelayedExchangeMessageScheduler();
                    cfg.UseConcurrencyLimit(int.Parse(rabbitMq["ConcurrencyLimit"] ?? "8"));
                    cfg.ConfigureEndpoints(ctx);
                    cfg.UseScheduledRedelivery(r => r.Interval(5, TimeSpan.FromDays(7)));
                    cfg.UseMessageRetry(r => r.None());
                });
            });
            services.AddDbContext<ErcContext>(options =>
               options.UseNpgsql(_configuration.GetConnectionString("ErcContext")));
            
            _serviceProvider = services.BuildServiceProvider();

            _busControl = _serviceProvider.GetRequiredService<IBusControl>();

            await _busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
            _logger.LogInformation("Bus started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync();
        }
    }
}
