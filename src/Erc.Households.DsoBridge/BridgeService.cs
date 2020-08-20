using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Zt.Energy.Dso;

namespace Erc.Households.DsoBridge
{
    public class BridgeService : IHostedService
    {
        readonly IBusControl _busControl;
       

        public BridgeService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync();
        }
    }
}
