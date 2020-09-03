using Erc.Households.BranchOfficeManagment.Core;
using Erc.Households.BranchOfficeManagment.EF;
using Erc.Households.Domain;
using Erc.Households.Domain.Billing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.BranchOfficeManagment
{
    public class BranchOfficeService : IBranchOfficeService
    {
        readonly BranchOfficeDbContext _dbContext;
        readonly IEnumerable<BranchOffice> _branchOffices;
        readonly List<Period> _periods;
        readonly object _sync = new object();

        public BranchOfficeService(BranchOfficeDbContext dbContext)
        {
            _dbContext = dbContext;
            _periods = _dbContext.Periods.ToList();
            _branchOffices = _dbContext.BranchOffices.ToArray();
        }

        public IEnumerable<BranchOffice> GetList(params int[] branchOfficeIds)
        {
            lock (_sync)
            {
                return _branchOffices
                    .Where(b => branchOfficeIds.Contains(b.Id)).ToArray();
            }
        }

        public IEnumerable<BranchOffice> GetList(IEnumerable<string> branchOfficeIds)
        {
            lock (_sync)
            {
                return _branchOffices
                    .Where(b => branchOfficeIds.Contains(b.StringId)).ToArray();
            }
        }

        public BranchOffice GetOne(int id)
        {
            lock (_sync)
            {
                return _branchOffices
                    .First(b => b.Id == id);
            }
        }

        public void StartNewPeriod(int branchOfficeId)
        {
            lock (_sync)
            {
                var branchOffice = _branchOffices.First(b => b.Id == branchOfficeId);
                var period = _periods.FirstOrDefault(p => p.StartDate == branchOffice.CurrentPeriod.EndDate.AddDays(1));

                using var transaction = _dbContext.Database.BeginTransaction();

                if (period is null)
                {
                    period = new Period(branchOffice.CurrentPeriod.EndDate.AddDays(1), branchOffice.CurrentPeriod.EndDate.AddMonths(1));
                    _dbContext.Entry(period).State = EntityState.Added;
                    _periods.Add(period);
                }

                branchOffice.StartNewPeriod(period);
                _dbContext.SaveChanges();
                _dbContext.Database.ExecuteSqlInterpolated($"insert into accounting_point_debt_history(accounting_point_id, period_id, debt_value) select id, {period.Id}, debt from accounting_points where branch_office_Id={branchOfficeId}");

                transaction.Commit();
            }
        }
    }
}
