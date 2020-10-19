using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace Erc.Households.Calculation
{
    public class CalculationService : IHostedService
    {
        readonly IBusControl _busControl;

        public CalculationService(IBusControl busControl) => _busControl = busControl;

        public async Task StartAsync(CancellationToken cancellationToken) => await _busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);

        public async Task StopAsync(CancellationToken cancellationToken) => await _busControl.StopAsync();
    }
}
