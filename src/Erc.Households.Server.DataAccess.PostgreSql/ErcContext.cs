using Erc.Households.Server.Domain;
using Erc.Households.Server.Domain.AccountingPoints;
using Erc.Households.Server.Domain.Addresses;
using Erc.Households.Server.Domain.Tariffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Erc.Households.Server.DataAccess.PostgreSql
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
        public DbSet<City> Cities { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Person> People { get; set; }

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
                e.HasData(
                    new { Id = 1, Name = "Андрушівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "an", DistrictIds = new[] { 1 } },
                    new { Id = 2, Name = "Баранiвський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "bn", DistrictIds = new[] { 2 } },
                    new { Id = 3, Name = "Бердичiвський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "bd", DistrictIds = new[] { 3 } },
                    new { Id = 4, Name = "Брусилівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "br", DistrictIds = new[] { 4 } },
                    new { Id = 5, Name = "Хорошівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "hr", DistrictIds = new[] { 5 } },
                    new { Id = 6, Name = "Ємільчинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "em", DistrictIds = new[] { 6 } },
                    new { Id = 7, Name = "Житомирський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "zt", DistrictIds = new[] { 7 } },
                    new { Id = 8, Name = "Зарічанський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "zr", DistrictIds = new[] { 7 } },
                    new { Id = 9, Name = "Коростенський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "kr", DistrictIds = new[] { 8, 10 } },
                    new { Id = 10, Name = "Коростишiвський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "kt", DistrictIds = new[] { 9 } },
                    new { Id = 11, Name = "Любарський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "lb", DistrictIds = new[] { 11 } },
                    new { Id = 12, Name = "Малинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ml", DistrictIds = new[] { 12 } },
                    new { Id = 13, Name = "Народицький ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "nr", DistrictIds = new[] { 13 } },
                    new { Id = 14, Name = "Новоград-Волинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "nv", DistrictIds = new[] { 14 } },
                    new { Id = 15, Name = "Овруцький ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ov", DistrictIds = new[] { 15 } },
                    new { Id = 16, Name = "Олевський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ol", DistrictIds = new[] { 16 } },
                    new { Id = 17, Name = "Попільнянський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "pp", DistrictIds = new[] { 17, 20 } },
                    new { Id = 18, Name = "Радомишльський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "rd", DistrictIds = new[] { 18 } },
                    new { Id = 19, Name = "Романівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "rm", DistrictIds = new[] { 19 } },
                    new { Id = 20, Name = "Пулинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "pl", DistrictIds = new[] { 21 } },
                    new { Id = 21, Name = "Черняхівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ch", DistrictIds = new[] { 22 } },
                    new { Id = 22, Name = "Чуднівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "cd", DistrictIds = new[] { 23 } }
                    );
            });

            modelBuilder.Entity<District>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(100).IsRequired();
                e.HasOne(p => p.Region)
                    .WithMany(r => r.Districts)
                    .HasForeignKey(p => p.RegionId);

                e.HasData(
                    new { Id = 1, Name = "Андрушівський р-н", RegionId = 1 },
                    new { Id = 2, Name = "Баранiвський р-н", RegionId = 1 },
                    new { Id = 3, Name = "Бердичiвський р-н", RegionId = 1 },
                    new { Id = 4, Name = "Брусилівський р-н", RegionId = 1 },
                    new { Id = 5, Name = "Хорошівський р-н", RegionId = 1 },
                    new { Id = 6, Name = "Ємільчинський р-н", RegionId = 1 },
                    new { Id = 7, Name = "Житомирський р-н", RegionId = 1 },
                    new { Id = 8, Name = "Коростенський р-н", RegionId = 1 },
                    new { Id = 9, Name = "Коростишiвський р-н", RegionId = 1 },
                    new { Id = 10, Name = "Лугинський р-н", RegionId = 1 },
                    new { Id = 11, Name = "Любарський р-н", RegionId = 1 },
                    new { Id = 12, Name = "Малинський р-н", RegionId = 1 },
                    new { Id = 13, Name = "Народицький р-н", RegionId = 1 },
                    new { Id = 14, Name = "Новоград-Волинський р-н", RegionId = 1 },
                    new { Id = 15, Name = "Овруцький р-н", RegionId = 1 },
                    new { Id = 16, Name = "Олевський р-н", RegionId = 1 },
                    new { Id = 17, Name = "Попільнянський р-н", RegionId = 1 },
                    new { Id = 18, Name = "Радомишльський р-н", RegionId = 1 },
                    new { Id = 19, Name = "Романівський р-н", RegionId = 1 },
                    new { Id = 20, Name = "Ружинський р-н", RegionId = 1 },
                    new { Id = 21, Name = "Пулинський р-н", RegionId = 1 },
                    new { Id = 22, Name = "Черняхівський р-н", RegionId = 1 },
                    new { Id = 23, Name = "Чуднівський р-н", RegionId = 1 }
                    );
            });

            modelBuilder.Entity<Region>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(100).IsRequired();
                e.HasData(new { Id = 1, Name = "Житомирська обл." });
            });

            modelBuilder.Entity<City>(e =>
            {
                e.Property(p => p.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                e.HasOne(p => p.District)
                    .WithMany();

                e.Property(p => p.IsDistrictCity).HasDefaultValue(false);
                e.Property(p => p.IsRegionCity).HasDefaultValue(false);
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

                e.HasIndex(p => new { p.StreetId, p.Building, p.Apt })
                .IsUnique();
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
                    .IsRequired()
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
                e.HasData(new { Id = 1, Name = "АТ «Житомиробленерго»" }, new { Id = 2, Name = "АТ «Укрзалізниця»" });
            });

            modelBuilder.Entity<Tariff>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(200);
                e.HasMany(p => p.Rates).WithOne();
                e.HasData(
                     new { Id = 1, Name = "Населення (загальний тариф)" },
                     new { Id = 2, Name = "Будинки з електроопалювальними установками" },
                     new { Id = 3, Name = "Багатоквартирні негазифіковані будинки" },
                     new { Id = 4, Name = "Багатодітні, прийомні сім''ї та дитячі будинкі сімейного типу" }
                    );
            });

            modelBuilder.Entity<TariffRate>(e =>
            {
                e.ToTable("tariff_rates")
                    .Property(p => p.Value).HasColumnType("decimal(8,5)");

                e.HasData(
                    new TariffRate { Id = 1, StartDate = new DateTime(2017, 3, 1), Value = 0.9m, ConsumptionLimit = 100, TariffId = 1 },
                    new TariffRate { Id = 2, StartDate = new DateTime(2017, 3, 1), Value = 1.68m, TariffId = 1 },
                    new TariffRate { Id = 3, StartDate = new DateTime(2017, 3, 1), Value = 0.90m, ConsumptionLimit = 100, HeatingConsumptionLimit = 3000, HeatingStartDay = new DateTime(2019, 10, 01), HeatingEndDay = new DateTime(2020, 04, 30), TariffId = 2 },
                    new TariffRate { Id = 4, StartDate = new DateTime(2017, 3, 1), Value = 1.68m, TariffId = 2 },
                    new TariffRate { Id = 5, StartDate = new DateTime(2017, 3, 1), Value = 0.90m, ConsumptionLimit = 100, HeatingConsumptionLimit = 3000, HeatingStartDay = new DateTime(2019, 10, 01), HeatingEndDay = new DateTime(2020, 04, 30), TariffId = 3 },
                    new TariffRate { Id = 6, StartDate = new DateTime(2017, 3, 1), Value = 1.68m, TariffId = 3 },
                    new TariffRate { Id = 7, StartDate = new DateTime(2017, 3, 1), Value = 0.90m, TariffId = 4 }
                    );
            });
        }
    }
}
