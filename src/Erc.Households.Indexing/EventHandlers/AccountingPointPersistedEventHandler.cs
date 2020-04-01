using Erc.Households.Server.Events.AccountingPoints;
using MassTransit;
using Nest;
using System.Threading.Tasks;

namespace Erc.Households.Indexing.EventHandlers
{
    public class AccountingPointPersistedEventHandler: IConsumer<AccountingPointCreated>
    {
        readonly IElasticClient _elasticClient;

        public AccountingPointPersistedEventHandler(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task Consume(ConsumeContext<AccountingPointCreated> context)
        {
            await _elasticClient.IndexAsync(context.Message, idx => idx.Index($"{context.Message.BranchOfficeStringId}_accounting_points"));
        }
    }
}
