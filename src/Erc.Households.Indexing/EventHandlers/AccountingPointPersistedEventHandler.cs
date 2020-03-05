using Erc.Households.Server.Domain.Events;
using MassTransit;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Erc.Households.Indexing.EventHandlers
{
    public class AccountingPointPersistedEventHandler: IConsumer<IAccountingPointPersisted>
    {
        readonly IElasticClient _elasticClient;

        public AccountingPointPersistedEventHandler(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task Consume(ConsumeContext<IAccountingPointPersisted> context)
        {
            await _elasticClient.IndexAsync(context.Message, idx => idx.Index($"{context.Message.BranchOfficeStringId}_accounting_points"));
        }
    }
}
