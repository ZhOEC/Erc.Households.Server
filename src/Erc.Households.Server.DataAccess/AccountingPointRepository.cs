using Erc.Households.Server.Core;
using Erc.Households.Server.DataAccess.EF;
using Erc.Households.Server.Domain.AccountingPoints;
using Erc.Households.Server.Events.AccountingPoints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Erc.Households.Server.DataAccess
{
    public class AccountingPointRepository : IAccountingPointRepository
    {
        readonly ErcContext _ercContext;

        public AccountingPointRepository(ErcContext ercContext) => _ercContext = ercContext;

        public async Task AddNewAsync(AccountingPoint accountingPoint)
        {
            await _ercContext.AccountingPoints.AddAsync(accountingPoint);
            if (accountingPoint.Owner.Id == 0)
                _ercContext.Entry(accountingPoint.Owner).State = EntityState.Added;
            else
            {
                _ercContext.Entry(accountingPoint.Owner).State = EntityState.Unchanged;
                _ercContext.Entry(accountingPoint.Owner).Property(p => p.IdCardExpDate).IsModified = true;
                _ercContext.Entry(accountingPoint.Owner).Property(p => p.IdCardNumber).IsModified = true;
                _ercContext.Entry(accountingPoint.Owner).Property(p => p.IdCardIssuanceDate).IsModified = true;
                _ercContext.Entry(accountingPoint.Owner).Property(p => p.MobilePhones).IsModified = true;
            }
            
            if (accountingPoint.AddressId == 0)
            {
                _ercContext.Entry(accountingPoint.Address).State = EntityState.Added;
            }

            accountingPoint.Events.Add(new AccountingPointCreated
            {
                Address = accountingPoint.Address.ToString(),
                Eic = accountingPoint.Eic,
                Name = accountingPoint.Name,
                PersonFirstName = accountingPoint.Owner.FirstName,
                PersonIdCardNumber = accountingPoint.Owner.IdCardNumber,
                PersonLastName = accountingPoint.Owner.LastName,
                PersonPatronymic = accountingPoint.Owner.Patronymic,
                PersonTaxCode = accountingPoint.Owner.TaxCode,
                BranchOfficeStringId = accountingPoint.BranchOffice.StringId
            });
        }

        public Task<AccountingPoint> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountingPoint> GetAsync(string eic)
        {
            throw new NotImplementedException();
        }
    }
}
