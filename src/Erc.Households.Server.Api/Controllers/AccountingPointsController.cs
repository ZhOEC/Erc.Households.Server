using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Server.Api.Authorization;
using Erc.Households.Server.Core;
using Erc.Households.Server.Domain.AccountingPoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = ApplicationRoles.User)]
    public partial class AccountingPointsController: ErcControllerBase
    {
        private readonly IElasticClient _elasticClient;
        readonly IUnitOfWork _unitOfWork;

        public AccountingPointsController(IElasticClient elasticClient, IUnitOfWork unitOfWork)
        {
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
            _unitOfWork = unitOfWork;
        }

        [HttpGet("_search")]
        public async Task<IActionResult> Search(string q)
        {
            var indices = string.Join(',', UserGroups.Select(c => c + "_accounting_points"));
           
            var searchResponse = await _elasticClient.SearchAsync<SearchResult>(s => s
                .Index(indices).Query(query => query
                    .MultiMatch(m => m
                        .Query(q)
                        .Operator(Operator.And)
                        .Type(TextQueryType.CrossFields)
                        )
                    )
                );

            var searchResults = searchResponse.Hits.Select(h =>
                new 
                {
                    h.Source.BranchOfficeName,
                    h.Source.StreetAddress,
                    h.Source.CityName,
                    h.Source.Eic,
                    h.Source.Id,
                    h.Source.Name,
                    Owner = $"{h.Source.PersonLastName} {h.Source.PersonFirstName} {h.Source.PersonPatronymic}"
                });

            return Ok(searchResults);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddNew(Requests.NewAccountingPoint newAccountingPoint)
        {
            var accountingPoint = new AccountingPoint(
                newAccountingPoint.Eic, newAccountingPoint.Name, newAccountingPoint.ContractStartDate, newAccountingPoint.TariffId,
                newAccountingPoint.Address, newAccountingPoint.Owner, newAccountingPoint.BranchOfficeId, newAccountingPoint.DsoId, User.Identity.Name
                );
            await _unitOfWork.AccountingPointRepository.AddNewAsync(accountingPoint);
            await _unitOfWork.SaveWorkAsync();

            return CreatedAtRoute("GetAccountingPoint", new { accountingPoint.Id }, null);
        }

        [HttpGet("{id}", Name= "GetAccountingPoint")]
        public async Task<IActionResult> Get(int id)
        {
            var ap = await _unitOfWork.AccountingPointRepository.GetAsync(id);
            return Ok(new 
            {
                ap.Id,
                ap.Name,
                Address = ap.Address.ToString(),
                ap.Owner,
                Dso = ap.Dso.Name
            });
        }

        class SearchResult
        {
            public int Id { get; set; }
            public string BranchOfficeName { get; set; }
            public string Name { get; set; }
            public string Eic { get; set; }
            public string PersonFirstName { get; set; }
            public string PersonLastName { get; set; }
            public string PersonPatronymic { get; set; }
            public string PersonTaxCode { get; set; }
            public string PersonIdCardNumber { get; set; }
            public string CityName { get; set; }
            public string StreetAddress { get; set; }
        }
    }
}