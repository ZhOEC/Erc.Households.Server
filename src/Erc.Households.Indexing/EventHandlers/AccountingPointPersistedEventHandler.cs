using Erc.Households.Events.AccountingPoints;
using MassTransit;
using Nest;
using System.Threading.Tasks;

namespace Erc.Households.Indexing.EventHandlers
{
    public class AccountingPointEventHandler: IConsumer<AccountingPointCreated>, IConsumer<AccountingPointUpdated>
    {
        readonly IElasticClient _elasticClient;

        public AccountingPointEventHandler(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task Consume(ConsumeContext<AccountingPointCreated> context)
        {
            await _elasticClient.IndexAsync(context.Message, idx => idx.Index($"{context.Message.BranchOfficeStringId}_accounting_points"));
        }

        public async Task Consume(ConsumeContext<AccountingPointUpdated> context)
        {
            await _elasticClient.IndexAsync(context.Message, idx => idx.Index($"{context.Message.BranchOfficeStringId}_accounting_points"));
        }
    }
}
