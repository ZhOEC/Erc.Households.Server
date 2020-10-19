using Erc.Households.DataAccess.Core;
using Erc.Households.EF.PostgreSQL;
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Events.AccountingPoints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Domain.Exceptions;

namespace Erc.Households.DataAccess.EF
{
    public class AccountingPointRepository : IAccountingPointRepository
    {
        readonly ErcContext _ercContext;

        public AccountingPointRepository(ErcContext ercContext) => _ercContext = ercContext;

        public async Task AddNewAsync(AccountingPoint accountingPoint)
        {
            _ercContext.Entry(accountingPoint.Owner).State = accountingPoint.Owner.Id == 0 ? EntityState.Added : EntityState.Modified;

            accountingPoint.Address.Id = 
                (await _ercContext.Addresses
                .Where(a => a.StreetId == accountingPoint.Address.StreetId && a.Building == accountingPoint.Address.Building && ((a.Apt ?? string.Empty) == (accountingPoint.Address.Apt ?? string.Empty)))
                .Select(a => (int?)a.Id)
                .FirstOrDefaultAsync()) ?? 0;

            _ercContext.Entry(accountingPoint.Address).State = accountingPoint.Address.Id == 0 ? EntityState.Added : EntityState.Unchanged;
            _ercContext.Entry(accountingPoint.Address).Property(p => p.Zip).IsModified = true;

            await _ercContext.AccountingPoints.AddAsync(accountingPoint);
            await _ercContext.Entry(accountingPoint).Reference(a => a.DistributionSystemOperator).LoadAsync();
           await _ercContext.Entry(accountingPoint.TariffsHistory.First()).Reference(a => a.Tariff).LoadAsync();
            await _ercContext.Entry(accountingPoint).Reference(a => a.BranchOffice).LoadAsync();

            if (accountingPoint.DistributionSystemOperator.Commodity != accountingPoint.Commodity
               || accountingPoint.CurrentTariff.Commodity != accountingPoint.Commodity || !accountingPoint.BranchOffice.AvailableCommodities.Contains(accountingPoint.Commodity))
                throw new InvalidAccountingPointDataException($"Invalid initial data: Commodity, Tariff.Commodity, DistributionSystemOperator.Commodity and BranchOffice.AvailableCommodities");

            accountingPoint.Events.Add(new AccountingPointCreated
            {
                StreetAddress = accountingPoint.Address.StreetLocation,
                CityName = accountingPoint.Address.CityName,
                Eic = accountingPoint.Eic,
                Name = accountingPoint.Name,
                PersonFirstName = accountingPoint.Owner.FirstName,
                PersonIdCardNumber = accountingPoint.Owner.IdCardNumber,
                PersonLastName = accountingPoint.Owner.LastName,
                PersonPatronymic = accountingPoint.Owner.Patronymic,
                PersonTaxCode = accountingPoint.Owner.TaxCode,
                BranchOfficeStringId = accountingPoint.BranchOffice.StringId,
                BranchOfficeName = accountingPoint.BranchOffice.Name
            });
        }
        
        public async Task<AccountingPoint> GetAsync(int id)
        {
            return await _ercContext.AccountingPoints
                .Include(a => a.TariffsHistory)
                    .ThenInclude(th => th.Tariff)
                .Include(a => a.ContractsHistory)
                     .ThenInclude(c => c.Customer)
                .Include(a => a.Address.Street.City.District.Region)
                .Include(a => a.DistributionSystemOperator)
                .Include(a => a.BranchOffice)
                .FirstAsync(a => a.Id == id);
        }

        public Task<AccountingPoint> GetAsync(string eic)
        {
            throw new NotImplementedException();
        }
    }
}
