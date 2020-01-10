using Erc.Households.Backend.Data;
using Erc.Households.Backend.Data.Addresses;
using Erc.Households.Backend.Data.Tariffs;
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

        public DbSet<AccountingPoint> AccountingPoints { get; set; }
        public DbSet<DistributionSystemOperator> DistributionSystemOperators { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
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
                e.Property(p => p.Name).HasMaxLength(100).IsRequired();
                e.HasOne(p => p.Region)
                    .WithMany(r => r.Districts)
                    .HasForeignKey(p => p.RegionId);
            });

            modelBuilder.Entity<Region>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<City>(e =>
            {
                e.Property(p => p.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                e.HasOne(p => p.District)
                    .WithMany();
            });

            modelBuilder.Entity<Street>(e =>
            {
                e.Property(p => p.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                e.HasOne(p => p.City)
                    .WithMany();
            });

            modelBuilder.Entity<Address>(e =>
            {
                e.Property(p => p.Building)
                    .HasMaxLength(20)
                    .IsRequired();

                e.Property(p => p.Apt)
                    .HasMaxLength(5);

                e.HasOne(p => p.Street)
                    .WithMany();
            });

            modelBuilder.Entity<Person>(e =>
            {
                e.Property(p => p.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(p => p.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(p => p.Patronymic)
                    .HasMaxLength(50);

                e.Property(p => p.IdCardNumber)
                    .HasMaxLength(9);

                e.Property(p => p.MobilePhone1)
                    .HasMaxLength(15);

                e.Property(p => p.MobilePhone2)
                    .HasMaxLength(15);

                e.Property(p => p.TaxCode)
                    .HasMaxLength(10);

                e.HasOne(p => p.Address)
                    .WithMany();

                e.HasIndex(p => p.TaxCode).IsUnique();
                e.HasIndex(p => p.IdCardNumber).IsUnique();
            });

            modelBuilder.Entity<AccountingPoint>(e =>
            {
                e.Property(p => p.Name)
                    .HasMaxLength(16)
                    .IsRequired();

                e.Property(p => p.Eic)
                    .HasMaxLength(16)
                    .IsRequired();

                e.HasOne(p => p.Owner)
                    .WithMany()
                    .HasForeignKey(p => p.OwnerId);

                e.HasOne(p => p.Tariff)
                   .WithMany();

                e.HasOne(p => p.Address)
                    .WithMany();

                e.HasOne(p => p.Dso)
                    .WithMany()
                    .HasForeignKey(p => p.DistributionSystemOperatorId);

                e.Property(p => p.Id).HasIdentityOptions(10000000);

                e.HasIndex(p => p.Name).IsUnique();
                e.HasIndex(p => p.Eic).IsUnique();
            });

            modelBuilder.Entity<DistributionSystemOperator>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Tariff>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(200);
                e.HasMany(p => p.Rates).WithOne();
            });

            modelBuilder.Entity<TariffRate>(e =>
            {
                e.ToTable("tariff_rates")
                    .Property(p => p.Value).HasColumnType("decimal(8,5)");
            });
        }
    }
}
