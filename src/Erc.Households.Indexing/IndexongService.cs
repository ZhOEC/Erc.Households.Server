using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erc.Households.Indexing
{
    public class IndexongService :IHostedService
    {
        private readonly ILogger<IndexongService> _logger;
        private readonly IBusControl _busControl;

        public IndexongService(ILogger<IndexongService> logger, IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
            _logger.LogInformation("Indexing service stopped");
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync();
            _logger.LogInformation("Indexing service started");
        }
    }
}
