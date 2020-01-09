using Erc.Households.Backend.Data;
using Erc.Households.Backend.Data.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Erc.Households.Backend.DataAccess.PostgreSql
{
    public class ErcContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); });

        public ErcContext(DbContextOptions<ErcContext> options) : base(options)
        {
        }

        public DbSet<BranchOffice> BranchOffices { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention().UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BranchOffice>(e =>
            {
                e.Property(p => p.StringId).HasMaxLength(2).IsRequired();
                e.Property(p => p.Address).HasMaxLength(500).IsRequired();
                e.Property(p => p.Name).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<District>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(200).IsRequired();
                e.HasOne(p => p.Region)
                    .WithMany(r => r.Districts)
                    .HasForeignKey(p => p.RegionId);
            });

            modelBuilder.Entity<Region>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(200).IsRequired();
            });
        }
    }
}
