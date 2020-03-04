using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erc.Households.Indexing
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                //var config = _configuration.GetSection("RabbitMQ").Get<RabbitMQ>();
                cfg.Host(new Uri("amqp://ztoec-rabbitmq:5672/ztoec"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("add_record_point", e =>
                {
                    e.Consumer(() => new AddRecordPoint(_configuration));
                });
            });

            busControl.Start();
            _logger.LogInformation("Consumer started");

            return base.StartAsync(cancellationToken);
        }
    }
}
