using Erc.Households.BranchOfficeManagment.Core;
using Erc.Households.Domain;
using Erc.Households.Domain.Billing;
using Erc.Households.EF.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.BranchOfficeManagment.EF
{
    public class BranchOfficeService : IBranchOfficeService
    {
        readonly ErcContext _ercContext;
        readonly IEnumerable<BranchOffice> _branchOffices;
        readonly object _sync = new object();

        public BranchOfficeService(ErcContext ercContext)
        {
            _ercContext = ercContext;
            _branchOffices = _ercContext.BranchOffices.Include(b => b.CurrentPeriod).ToArray();
        }

        public IEnumerable<BranchOffice> GetList(params int[] branchOfficeIds)
        {
            lock (_sync)
            {
                return _branchOffices.Where(b => branchOfficeIds.Contains(b.Id));
            }
        }

        public IEnumerable<BranchOffice> GetList(IEnumerable<string> branchOfficeIds)
        {
            lock (_sync)
            {
                return _branchOffices.Where(b => branchOfficeIds.Contains(b.StringId));
            }
        }

        public BranchOffice GetOne(int id)
        {
            lock (_sync)
            {
                return _branchOffices.First(b => b.Id == id);
            }
        }

        public void StartNewPeriod(int branchOfficeId)
        {
            lock (_sync)
            {
                var branchOffice = _branchOffices.First(b => b.Id == branchOfficeId);
                var period = _branchOffices
                    .FirstOrDefault(b => b.CurrentPeriod.StartDate == branchOffice.CurrentPeriod.EndDate.AddDays(1))
                    ?.CurrentPeriod;

                using var transaction = _ercContext.Database.BeginTransaction();

                if (period is null)
                {
                    period = new Period(branchOffice.CurrentPeriod.EndDate.AddDays(1), branchOffice.CurrentPeriod.EndDate.AddMonths(1));
                    _ercContext.Entry(period).State = EntityState.Added;
                }

                branchOffice.StartNewPeriod(period);
                _ercContext.SaveChanges();
                _ercContext.Database.ExecuteSqlInterpolated($"insert into accounting_point_balance_histories(accounting_point_id, period_id, balance_value) select id, {period.Id}, debt from accounting_points");

                transaction.Commit();
            }
        }
    }
}
