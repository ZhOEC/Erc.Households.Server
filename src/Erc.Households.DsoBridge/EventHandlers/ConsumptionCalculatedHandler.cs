using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Billing;
using Erc.Households.EF.PostgreSQL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zt.Energy.Dso.Events;

namespace Erc.Households.Calculation.EventHandlers
{
    public class ConsumptionCalculatedHandler : IConsumer<ConsumptionCalculated>
    {
        

        public ConsumptionCalculatedHandler(IBus bus)
        {
           
        }

        public async Task Consume(ConsumeContext<ConsumptionCalculated> context)
        {
            
            
        }
    }
}
