using Erc.Households.Server.Requests;
using Erc.Households.Server.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erc.Households.Searching.RequestHandlers
{
    class SearchAccountingPointRequestHandler : IConsumer<SearchAccountingPointRequest>
    {
        private readonly IElasticClient _elasticClient;
        ILogger<SearchingService> _logger;

        public SearchAccountingPointRequestHandler(ILogger<SearchingService> logger, IElasticClient elasticClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        }

        public async Task Consume(ConsumeContext<SearchAccountingPointRequest> context)
        {
            var indices = string.Join(',', context.Message.BranchOffices.Where(b => b != "bn").Select(b => b + "_accounting_points"));
            var searchResponse = await _elasticClient.SearchAsync<SearchResult>(s => s
                .Index(indices).Query(q => q
                    .MultiMatch(m => m
                        .Query(context.Message.SearchString)
                        .Operator(Operator.And)
                        .Type(TextQueryType.CrossFields)
                        )
                    )
                );

            _logger.LogDebug("Found {count} recordpoints", searchResponse.Hits.Count);
            var searchResults = searchResponse.Hits.Select(h =>
                new AccountingPointListItem
                {
                    Address = h.Source.Address,
                    Eic = h.Source.Eic,
                    Id = h.Source.Id,
                    Name = h.Source.Name,
                    Owner = $"{h.Source.PersonLastName} {h.Source.PersonFirstName} {h.Source.PersonPatronymic}"
                });
            _logger.LogDebug("Sending response...");
            await context.RespondAsync(searchResults.ToArray());
        }

        class SearchResult
        {
            public int Id { get; set; }
            public string BranchOfficeStringId { get; set; }
            public string Name { get; set; }
            public string Eic { get; set; }
            public string PersonFirstName { get; set; }
            public string PersonLastName { get; set; }
            public string PersonPatronymic { get; set; }
            public string PersonTaxCode { get; set; }
            public string PersonIdCardNumber { get; set; }
            public string Address { get; set; }
        }
    }
}
