using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Erc.Households.Calculations.Gas
{
    public class GasCalculationService : IHostedService
    {
        readonly IBusControl _busControl;

        public GasCalculationService(IBusControl busControl) => _busControl = busControl;

        public async Task StartAsync(CancellationToken cancellationToken) => await _busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);

        public async Task StopAsync(CancellationToken cancellationToken) => await _busControl.StopAsync();
    }
}
