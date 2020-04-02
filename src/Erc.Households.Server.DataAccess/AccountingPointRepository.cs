using Erc.Households.Server.Core;
using Erc.Households.Server.DataAccess.PostgreSql;
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
            _ercContext.Entry(accountingPoint.Owner).State = accountingPoint.Owner.Id == 0 ? EntityState.Added : EntityState.Unchanged;
            if (accountingPoint.AddressId == 0)
            {
                _ercContext.Entry(accountingPoint.Address).State = EntityState.Added;
            }

            accountingPoint.Events.Add(new AccountingPointCreated
            {
                //Address = accountingPoint.Address.ToString(),
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
