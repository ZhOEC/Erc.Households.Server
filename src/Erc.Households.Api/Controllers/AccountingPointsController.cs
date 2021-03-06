﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Api.Authorization;
using Erc.Households.Api.Queries.AccountingPoints;
using Erc.Households.Api.Requests;
using Erc.Households.Commands;
using Erc.Households.DataAccess.Core;
using Erc.Households.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ApplicationRoles.Operator)]
    public partial class AccountingPointsController: ErcControllerBase
    {
        private readonly IElasticClient _elasticClient;
        readonly IUnitOfWork _unitOfWork;
        IMediator _mediator;

        public AccountingPointsController(IElasticClient elasticClient, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        [HttpGet("_search")]
        public async Task<IActionResult> Search(string q)
        {
          var indices = string.Join(',', UserGroups.Select(c => c + "_accounting_points"));
            //indices = "_all"; 
            var searchResponse = await _elasticClient.SearchAsync<SearchResult>(s => s
                .Index(indices).Query(query => query
                    .MultiMatch(m => m
                        .Query(q)
                        .Operator(Operator.And)
                        .Type(TextQueryType.CrossFields)
                        )
                    )
                .IgnoreUnavailable()
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
        public async Task<IActionResult> AddNew(NewAccountingPoint newAccountingPoint)
        {
            var accountingPoint = new Domain.AccountingPoints.AccountingPoint(
                newAccountingPoint.Eic, newAccountingPoint.Name, newAccountingPoint.Commodity, newAccountingPoint.ContractStartDate,
                newAccountingPoint.TariffId, newAccountingPoint.Address, newAccountingPoint.Owner,
                newAccountingPoint.BranchOfficeId, newAccountingPoint.DistributionSystemOperatorId, User.Identity.Name, newAccountingPoint.BuildingTypeId,
                newAccountingPoint.UsageCategoryId, newAccountingPoint.SendPaperBill);

            await _unitOfWork.AccountingPointRepository.AddNewAsync(accountingPoint);
            await _unitOfWork.SaveWorkAsync();

            return CreatedAtRoute("GetAccountingPoint", new { accountingPoint.Id }, new { accountingPoint.Id });
        }

        [HttpGet("{id}", Name = "GetAccountingPoint")]
        public async Task<IActionResult> Get(string id) =>
            Ok(await _mediator.Send(new GetAccountingPointById(id)));
        
        [HttpPost("{id}/open-exemption")]
        public async Task<IActionResult> OpenExemption(int id, ExemptionOpening exemptionOpening)
        {
            await _mediator.Send(new OpenAccountingPointExemptionCommand(id, exemptionOpening.ExemptionCategoryId, exemptionOpening.Date, exemptionOpening.Certificate, exemptionOpening.PersonCount,
                exemptionOpening.Limit, exemptionOpening.Note, exemptionOpening.Person));
            await _unitOfWork.SaveWorkAsync();

            return Ok();
        }

        [HttpPost("{id}/closing-current-exemption")]
        public async Task<IActionResult> CloseCurrentExemption(int id, ExemptionClosing exemptionClosing)
        {
            await _mediator.Send(new CloseAccountingPointExemption(id, exemptionClosing.Date, exemptionClosing.Note));
            //await Task.Delay(TimeSpan.FromSeconds(3));
            await _unitOfWork.SaveWorkAsync();
            
            return Ok();
        }

        [HttpPost("{id}/open-new-contract")]
        public async Task<IActionResult> OpenNewContractObsolete(int id, NewContract newContract)
        {
            await _mediator.Send(new OpenNewContractCommand(id, newContract.Owner.Id, newContract.ContractStartDate, newContract.SendPaperBill,
                newContract.Owner.IdCardNumber, newContract.Owner.IdCardIssuanceDate, newContract.Owner.IdCardIssuer, newContract.Owner.IdCardExpDate, newContract.Owner.TaxCode,
                newContract.Owner.FirstName, newContract.Owner.LastName, newContract.Owner.Patronymic, newContract.Owner.MobilePhones, newContract.Owner.Email, User.Identity.Name));
            await _unitOfWork.SaveWorkAsync();

            return Ok();
        }

        [HttpPost("{id}/contract")]
        public async Task<IActionResult> OpenNewContract(int id, NewContract newContract) => 
            await _mediator.Send(new OpenNewContractCommand(id, newContract.Owner.Id, newContract.ContractStartDate,
                                                            newContract.SendPaperBill, newContract.Owner.IdCardNumber,
                                                            newContract.Owner.IdCardIssuanceDate,
                                                            newContract.Owner.IdCardIssuer,
                                                            newContract.Owner.IdCardExpDate, newContract.Owner.TaxCode,
                                                            newContract.Owner.FirstName, newContract.Owner.LastName,
                                                            newContract.Owner.Patronymic, newContract.Owner.MobilePhones,
                                                            newContract.Owner.Email, User.Identity.Name))
                .ContinueWith(_ => _unitOfWork.SaveWorkAsync())
                .Unwrap()
                .ContinueWith(_ => Ok());

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountingPoint(UpdatedAccountingPoint updatedAccountingPoint)
        {
            await _mediator.Send(new UpdateAccountingPointCommand(
                updatedAccountingPoint.Id, updatedAccountingPoint.Eic, updatedAccountingPoint.Name, updatedAccountingPoint.BranchOfficeId, updatedAccountingPoint.DistributionSystemOperatorId, 
                updatedAccountingPoint.Address.StreetId, updatedAccountingPoint.Address.Building, updatedAccountingPoint.Address.Apt, updatedAccountingPoint.Address.Zip, 
                updatedAccountingPoint.BuildingTypeId, updatedAccountingPoint.UsageCategoryId));

            await _unitOfWork.SaveWorkAsync();

            return Ok();
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