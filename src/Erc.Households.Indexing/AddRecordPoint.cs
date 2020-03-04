using Erc.Households.Server.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Threading.Tasks;

namespace Erc.Households.Indexing
{
    class AddRecordPoint : IConsumer<IAccountingPointPersisted>
    {
        private readonly IConfiguration _configuration;
        
        public AddRecordPoint(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Consume(ConsumeContext<IAccountingPointPersisted> context)
        {
            await Console.Out.WriteLineAsync($"Get id: {context.Message.Id}");

            var node = new Uri("http://ztoec-es:9200");
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);

            var accountingPoint = new
            {
                context.Message.Id,
                context.Message.AccountingPointName,
                context.Message.Eic,
                context.Message.PersonFirstName,
                context.Message.PersonLastName,
                context.Message.PersonPatronymic,
                context.Message.PersonTaxCode,
                context.Message.PersonIdCardNumber,
                context.Message.Address
            };

            await client.IndexAsync(accountingPoint, idx => idx.Index($"d_accounting_points_{context.Message.BranchOfficeStringId}"));
        }
    }
}
