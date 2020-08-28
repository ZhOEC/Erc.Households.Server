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
    public class CalculationService : IHostedService
    {
        readonly IBusControl _busControl;

        public CalculationService(IBusControl busControl) => _busControl = busControl;

        public async Task StartAsync(CancellationToken cancellationToken) => await _busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);

        public async Task StopAsync(CancellationToken cancellationToken) => await _busControl.StopAsync();
    }
}
