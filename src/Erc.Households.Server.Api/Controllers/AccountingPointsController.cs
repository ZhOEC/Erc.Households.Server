using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Erc.Households.Server.Api.Authorization;
using Erc.Households.Server.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ApplicationRoles.User)]
    public partial class AccountingPointsController: ErcControllerBase
    {
        private readonly IElasticClient _elasticClient;
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public AccountingPointsController(IElasticClient elasticClient, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            var accountingPoint = new Domain.AccountingPoints.AccountingPoint(
                newAccountingPoint.Eic, newAccountingPoint.Name, newAccountingPoint.ContractStartDate,
                newAccountingPoint.TariffId, newAccountingPoint.Address, newAccountingPoint.Owner,
                newAccountingPoint.BranchOfficeId, newAccountingPoint.DsoId, User.Identity.Name);

            await _unitOfWork.AccountingPointRepository.AddNewAsync(accountingPoint);
            await _unitOfWork.SaveWorkAsync();

            return CreatedAtRoute("GetAccountingPoint", new { accountingPoint.Id }, new { accountingPoint.Id });
        }

        [HttpGet("{id}", Name= "GetAccountingPoint")]
        public async Task<IActionResult> Get(int id)
        {
            var ap = await _unitOfWork.AccountingPointRepository.GetAsync(id);
            return Ok(_mapper.Map<Reponses.AccountingPoint>(ap));
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