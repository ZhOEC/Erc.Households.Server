using Erc.Households.Domain;
using Erc.Households.Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.BranchOfficeManagment.EF
{
    public class BranchOfficeDbContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); });
        public DbSet<BranchOffice> BranchOffices { get; set; }
        public DbSet<Period> Periods { get; set; }

        public BranchOfficeDbContext(DbContextOptions<BranchOfficeDbContext> options)
        : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention().UseLoggerFactory(MyLoggerFactory);
        }
    }
}
