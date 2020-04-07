using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Backend.Responses;
using Erc.Households.Server.Api.Authorization;
<<<<<<< .mine
using Erc.Households.Server.Core;
using Erc.Households.Server.DataAccess.EF;
using Erc.Households.Server.Domain.AccountingPoints;
using Erc.Households.Server.Domain.Addresses;
using Erc.Households.Server.Requests;
using Erc.Households.Server.Responses;
using MassTransit;
=======
using Erc.Households.Server.DataAccess.PostgreSql;
using Erc.Households.Server.Domain.AccountingPoints;





>>>>>>> .theirs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = ApplicationRoles.User)]
    public class AccountingPointsController : ControllerBase
    {
        private readonly ErcContext _ercContext;
        private readonly IMapper _mapper;
        private readonly IRequestClient<SearchAccountingPointRequest> _searchClient;
        private readonly IElasticClient _elasticClient;
        readonly IUnitOfWork _unitOfWork;

        public AccountingPointsController(IElasticClient elasticClient, IUnitOfWork unitOfWork, ErcContext ercContext)
        {
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
            _unitOfWork = unitOfWork;
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        //public AccountingPointsController(ErcContext ercContext, IMapper mapper, IRequestClient<SearchAccountingPointRequest> searchClient)
        //{
        //    _mapper = mapper;

        //    _searchClient = searchClient ?? throw new ArgumentNullException(nameof(searchClient));
        //}

        [HttpGet("")]
        public async Task<IActionResult> Search(string search)
        {
<<<<<<< .mine
            var indices = string.Join(',',
                User.Claims.Where(c => string.Equals(c.Type, "Group", StringComparison.InvariantCultureIgnoreCase))
                .Where(c => c.Value != "bn")
                .Select(c => c.Value + "_accounting_points")
                );





=======
            var keyWords = search.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            System.Linq.Expressions.Expression<Func<AccountingPoint, bool>> predicate = (a) => keyWords.Length > 1
               ? keyWords.Contains(a.Owner.LastName.ToLower()) && keyWords.Contains(a.Owner.FirstName.ToLower())
               : (
                   EF.Functions.ILike(a.Owner.LastName, $"{search}%")
                   || EF.Functions.ILike(a.Name, $"%{search}%")
                   || a.Owner.TaxCode.Contains(search)
                   || a.Owner.IdCardNumber.Contains(search)
               ) && (search.Length < 5 ? EF.Functions.ILike(a.Owner.LastName, search) : true);
>>>>>>> .theirs
            
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
<<<<<<< .mine

        [HttpPost("")]
        public async Task<IActionResult> AddNew(NewAccountingPoint newAccountingPoint)
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

        public class NewAccountingPoint
        {
            public string Eic { get; set; }
            public string Name { get; set; }
            public DateTime ContractStartDate { get; set; }
            public int TariffId { get; set; }
            public Address Address { get; set; }
            public Domain.Person Owner { get; set; }
            public int BranchOfficeId { get; set; }
            public int DsoId { get; set; }
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
=======

        [HttpGet("add-accounting-point")]
        public async Task<IActionResult> AddAsync(AccountingPoint accountingPoint)
        {
            await _ercContext.AccountingPoints.AddAsync(accountingPoint);
            return Ok();
        }






































>>>>>>> .theirs
    }
}