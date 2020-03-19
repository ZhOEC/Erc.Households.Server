using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Backend.Responses;
using Erc.Households.Server.Api.Authorization;
using Erc.Households.Server.DataAccess.PostgreSql;
using Erc.Households.Server.Requests;
using Erc.Households.Server.Responses;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ApplicationRoles.User)]
    public class AccountingPointsController : ControllerBase
    {
        private readonly ErcContext _ercContext;
        private readonly IMapper _mapper;
        private readonly IRequestClient<SearchAccountingPointRequest> _searchClient;
        private readonly IElasticClient _elasticClient;

        public AccountingPointsController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        }

        //public AccountingPointsController(ErcContext ercContext, IMapper mapper, IRequestClient<SearchAccountingPointRequest> searchClient)
        //{
        //    _mapper = mapper;
        //    _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        //    _searchClient = searchClient ?? throw new ArgumentNullException(nameof(searchClient));
        //}

        [HttpGet("")]
        public async Task<IActionResult> Search(string search)
        {
            var indices = string.Join(',',
                User.Claims.Where(c => string.Equals(c.Type, "Group", StringComparison.InvariantCultureIgnoreCase))
                .Where(c => c.Value != "bn")
                .Select(c => c.Value + "_accounting_points")
                );
            
            //var response = await _searchClient.GetResponse<AccountingPointListItem[]>(new { SearchString = search, BranchOffices = groups });

           
            var searchResponse = await _elasticClient.SearchAsync<SearchResult>(s => s
                .Index(indices).Query(q => q
                    .MultiMatch(m => m
                        .Query(search)
                        .Operator(Operator.And)
                        .Type(TextQueryType.CrossFields)
                        )
                    )
                );

           
            var searchResults = searchResponse.Hits.Select(h =>
                new AccountingPointListItem
                {
                    Address = h.Source.Address,
                    Eic = h.Source.Eic,
                    Id = h.Source.Id,
                    Name = h.Source.Name,
                    Owner = $"{h.Source.PersonLastName} {h.Source.PersonFirstName} {h.Source.PersonPatronymic}"
                });
          


            return Ok(searchResults);
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