using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Backend.Responses;
using Erc.Households.Server.Api.Authorization;
using Erc.Households.Server.DataAccess.PostgreSql;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Erc.Households.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ApplicationRoles.User)]
    public class AccountingPointsController : ControllerBase
    {
        private readonly ErcContext _ercContext;
        private readonly IMapper _mapper;

        public AccountingPointsController(ErcContext ercContext, IMapper mapper)
        {
            _mapper = mapper;
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet("")]
        public async Task<IActionResult> Search(string search, int? branchOfficeId)
        {
            var keyWords = search.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            System.Linq.Expressions.Expression<Func<Domain.AccountingPoints.AccountingPoint, bool>> predicate = (a) => keyWords.Length > 1
               ? keyWords.Contains(a.Owner.LastName.ToLower()) && keyWords.Contains(a.Owner.FirstName.ToLower())
               : (
                   EF.Functions.ILike(a.Owner.LastName, $"{search}%")
                   || EF.Functions.ILike(a.Name, $"%{search}%")
                   || a.Owner.TaxCode.Contains(search)
                   || a.Owner.IdCardNumber.Contains(search)
               ) && (search.Length < 5 ? EF.Functions.ILike(a.Owner.LastName, search) : true);
            
            var res = await _ercContext.AccountingPoints
                .Include(a => a.TariffsHistory)
                    .ThenInclude(t => t.Tariff)
                .Include(a => a.Address.Street.City)
                .Include(a => a.Owner)
                .Include(a => a.BranchOffice)
                .Where(a => a.BranchOfficeId == (branchOfficeId ?? a.BranchOfficeId))
                .Where(predicate)
                .ProjectTo<AccountingPointListItem>(_mapper.ConfigurationProvider).ToArrayAsync();
            //.Select(a=> new { a.Id, a.Name, a.Owner.LastName, a.Address.Street })

            return Ok(res);
        }
    }
}