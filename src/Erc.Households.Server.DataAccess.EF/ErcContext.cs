using Erc.Households.Domain;
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Addresses;
using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Payments;
using Erc.Households.Domain.Tariffs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Erc.Households.EF.PostgreSQL
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
        public DbSet<ZoneCoeff> ZoneCoeffs { get; set; }
        public DbSet<PaymentChannel> PaymentChannels { get; set; }
        public DbSet<PaymentsBatch> PaymentBatches { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention().UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.Entity<PaymentsBatch>(entity =>
            {
                entity.Property(e => e.IncomingDate).HasColumnType("date");
                
                entity.HasMany(pb => pb.Payments)
                .WithOne(p => p.Batch)
                .HasForeignKey(p => p.BatchId);
            });

            modelBuilder.Entity<Period>(entity =>
            {
                entity.ToTable("periods");
                entity.Property(e => e.StartDate).HasColumnType("date");
                entity.Property(e => e.EndDate).HasColumnType("date");
                entity.HasIndex(e => e.StartDate).IsUnique();
                entity.HasData(
                    new { Id = 201901, StartDate = new DateTime(2019, 1, 1), EndDate = new DateTime(2019, 1, 1).AddMonths(1).AddDays(-1), Name = "Січень 2019р."},
                    new { Id = 201902, StartDate = new DateTime(2019, 2, 1), EndDate = new DateTime(2019, 2, 1).AddMonths(1).AddDays(-1), Name = "Лютий 2019р." },
                    new { Id = 201903, StartDate = new DateTime(2019, 3, 1), EndDate = new DateTime(2019, 3, 1).AddMonths(1).AddDays(-1), Name = "Березень 2019р." },
                    new { Id = 201904, StartDate = new DateTime(2019, 4, 1), EndDate = new DateTime(2019, 4, 1).AddMonths(1).AddDays(-1), Name = "Квітень 2019р." },
                    new { Id = 201905, StartDate = new DateTime(2019, 5, 1), EndDate = new DateTime(2019, 5, 1).AddMonths(1).AddDays(-1), Name = "Травень 2019р." },
                    new { Id = 201906, StartDate = new DateTime(2019, 6, 1), EndDate = new DateTime(2019, 6, 1).AddMonths(1).AddDays(-1), Name = "Червень 2019р." },
                    new { Id = 201907, StartDate = new DateTime(2019, 7, 1), EndDate = new DateTime(2019, 7, 1).AddMonths(1).AddDays(-1), Name = "Липень 2019р." },
                    new { Id = 201908, StartDate = new DateTime(2019, 8, 1), EndDate = new DateTime(2019, 8, 1).AddMonths(1).AddDays(-1), Name = "Серпень 2019р." },
                    new { Id = 201909, StartDate = new DateTime(2019, 9, 1), EndDate = new DateTime(2019, 9, 1).AddMonths(1).AddDays(-1), Name = "Вересень 2019р." },
                    new { Id = 201910, StartDate = new DateTime(2019, 10, 1), EndDate = new DateTime(2019, 10, 1).AddMonths(1).AddDays(-1), Name = "Жовтень 2019р." },
                    new { Id = 201911, StartDate = new DateTime(2019, 11, 1), EndDate = new DateTime(2019, 11, 1).AddMonths(1).AddDays(-1), Name = "Листопад 2019р." },
                    new { Id = 201912, StartDate = new DateTime(2019, 12, 1), EndDate = new DateTime(2019, 12, 1).AddMonths(1).AddDays(-1), Name = "Грудень 2019р." },
                    new { Id = 202001, StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2019, 1, 1).AddMonths(1).AddDays(-1), Name = "Січень 2019р." },
                    new { Id = 202002, StartDate = new DateTime(2020, 2, 1), EndDate = new DateTime(2019, 2, 1).AddMonths(1).AddDays(-1), Name = "Лютий 2019р." },
                    new { Id = 202003, StartDate = new DateTime(2020, 3, 1), EndDate = new DateTime(2019, 3, 1).AddMonths(1).AddDays(-1), Name = "Березень 2019р." },
                    new { Id = 202004, StartDate = new DateTime(2020, 4, 1), EndDate = new DateTime(2019, 4, 1).AddMonths(1).AddDays(-1), Name = "Квітень 2019р." },
                    new { Id = 202005, StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2019, 5, 1).AddMonths(1).AddDays(-1), Name = "Травень 2019р." },
                    new { Id = 202006, StartDate = new DateTime(2020, 6, 1), EndDate = new DateTime(2019, 6, 1).AddMonths(1).AddDays(-1), Name = "Червень 2019р." }
                    );
                entity.Property(e => e.Name).HasColumnType("citext")
                    .IsRequired();
            });

            modelBuilder.Entity<PaymentChannel>(entity =>
            {
                entity.Property(e => e.Name).HasColumnType("citext")
                    .IsRequired();
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<ZoneCoeff>(entity =>
            {
                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasData(
                    new { Id = 1, ZoneNumber = ZoneNumber.T1, ZoneRecord = ZoneRecord.None, Value = 1m, DiscountWeight = 1m, StartDate = new DateTime(2019, 1, 1) },
                    new { Id = 2, ZoneNumber = ZoneNumber.T1, ZoneRecord = ZoneRecord.Two, Value = 0.5m, DiscountWeight = 0.67m, StartDate = new DateTime(2019, 1, 1) },
                    new { Id = 3, ZoneNumber = ZoneNumber.T2, ZoneRecord = ZoneRecord.Two, Value = 1m, DiscountWeight = 0.33m, StartDate = new DateTime(2019, 1, 1) },
                    new { Id = 4, ZoneNumber = ZoneNumber.T1, ZoneRecord = ZoneRecord.Three, Value = 0.4m, DiscountWeight = 0.46m, StartDate = new DateTime(2019, 1, 1) },
                    new { Id = 5, ZoneNumber = ZoneNumber.T2, ZoneRecord = ZoneRecord.Three, Value = 1m, DiscountWeight = 0.33m, StartDate = new DateTime(2019, 1, 1) },
                    new { Id = 6, ZoneNumber = ZoneNumber.T3, ZoneRecord = ZoneRecord.Three, Value = 1.5m, DiscountWeight = 0.21m, StartDate = new DateTime(2019, 1, 1) }
                    );
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(p => p.TotalAmountDue).HasColumnType("decimal(10,2)");
                entity.Property(p => p.TotalDiscount).HasColumnType("decimal(10,2)");

                entity.Property(e => e.FromDate).HasColumnType("date");
                entity.Property(e => e.ToDate).HasColumnType("date");
                entity.Property(b => b.UsageT1).HasColumnType("jsonb");
                entity.Property(b => b.UsageT2).HasColumnType("jsonb");
                entity.Property(b => b.UsageT3).HasColumnType("jsonb");
            });

            modelBuilder.Entity<Contract>(e =>
            {
                e.ToTable("contracts")
                    .Property(p => p.Logs).HasColumnType("jsonb");

                e.Property(e => e.StartDate).HasColumnType("date");
                e.Property(e => e.EndDate).HasColumnType("date");
            });

            modelBuilder.Entity<AccountingPointTariff>(e =>
            {
                e.ToTable("accounting_point_tariffs");

                e.Property(e => e.StartDate).HasColumnType("date");

                e.Property(p => p.Logs).HasColumnType("jsonb");
                
                e.HasOne(p => p.Tariff)
                    .WithMany()
                    .HasForeignKey(p => p.TariffId);
            });

            modelBuilder.Entity<BranchOffice>(e =>
            {
                e.Property(p => p.StringId)
                    .HasColumnType("citext")
                    .HasMaxLength(2).IsRequired();

                e.Property(p => p.Address)
                    .HasColumnType("citext")
                    .HasMaxLength(500).IsRequired();

                e.Property(p => p.Name)
                    .HasColumnType("citext")
                    .HasMaxLength(200).IsRequired();
                e.HasData(
                    new { Id = 1, CurrentPeriodId = 201901, Name = "Андрушівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "an", DistrictIds = new[] { 1 } },
                    new { Id = 2, CurrentPeriodId = 201901, Name = "Баранiвський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "bn", DistrictIds = new[] { 2 } },
                    new { Id = 3, CurrentPeriodId = 201901, Name = "Бердичiвський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "bd", DistrictIds = new[] { 3 } },
                    new { Id = 4, CurrentPeriodId = 201901, Name = "Брусилівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "br", DistrictIds = new[] { 4 } },
                    new { Id = 5, CurrentPeriodId = 201901, Name = "Хорошівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "hr", DistrictIds = new[] { 5 } },
                    new { Id = 6, CurrentPeriodId = 201901, Name = "Ємільчинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "em", DistrictIds = new[] { 6 } },
                    new { Id = 7, CurrentPeriodId = 201901, Name = "Житомирський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "zt", DistrictIds = new[] { 7 } },
                    new { Id = 8, CurrentPeriodId = 201901, Name = "Зарічанський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "zr", DistrictIds = new[] { 7 } },
                    new { Id = 9, CurrentPeriodId = 201901, Name = "Коростенський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "kr", DistrictIds = new[] { 8, 10 } },
                    new { Id = 10, CurrentPeriodId = 201901, Name = "Коростишiвський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "kt", DistrictIds = new[] { 9 } },
                    new { Id = 11, CurrentPeriodId = 201901, Name = "Любарський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "lb", DistrictIds = new[] { 11 } },
                    new { Id = 12, CurrentPeriodId = 201901, Name = "Малинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ml", DistrictIds = new[] { 12 } },
                    new { Id = 13, CurrentPeriodId = 201901, Name = "Народицький ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "nr", DistrictIds = new[] { 13 } },
                    new { Id = 14, CurrentPeriodId = 201901, Name = "Новоград-Волинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "nv", DistrictIds = new[] { 14 } },
                    new { Id = 15, CurrentPeriodId = 201901, Name = "Овруцький ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ov", DistrictIds = new[] { 15 } },
                    new { Id = 16, CurrentPeriodId = 201901, Name = "Олевський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ol", DistrictIds = new[] { 16 } },
                    new { Id = 17, CurrentPeriodId = 201901, Name = "Попільнянський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "pp", DistrictIds = new[] { 17, 20 } },
                    new { Id = 18, CurrentPeriodId = 201901, Name = "Радомишльський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "rd", DistrictIds = new[] { 18 } },
                    new { Id = 19, CurrentPeriodId = 201901, Name = "Романівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "rm", DistrictIds = new[] { 19 } },
                    new { Id = 20, CurrentPeriodId = 201901, Name = "Пулинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "pl", DistrictIds = new[] { 21 } },
                    new { Id = 21, CurrentPeriodId = 201901, Name = "Черняхівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ch", DistrictIds = new[] { 22 } },
                    new { Id = 22, CurrentPeriodId = 201901, Name = "Чуднівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "cd", DistrictIds = new[] { 23 } }
                    );
            });

            modelBuilder.Entity<District>(e =>
            {
                e.Property(p => p.Name)
                    .HasColumnType("citext")
                    .HasMaxLength(100)
                    .IsRequired();
                
                e.HasOne(p => p.Region)
                    .WithMany()
                    .HasForeignKey(p => p.RegionId);

                e.HasData(
                    new { Id = 1, Name = "Андрушівський район", RegionId = 1 },
                    new { Id = 2, Name = "Баранiвський район", RegionId = 1 },
                    new { Id = 3, Name = "Бердичiвський район", RegionId = 1 },
                    new { Id = 4, Name = "Брусилівський район", RegionId = 1 },
                    new { Id = 5, Name = "Хорошівський район", RegionId = 1 },
                    new { Id = 6, Name = "Ємільчинський район", RegionId = 1 },
                    new { Id = 7, Name = "Житомирський район", RegionId = 1 },
                    new { Id = 8, Name = "Коростенський район", RegionId = 1 },
                    new { Id = 9, Name = "Коростишiвський район", RegionId = 1 },
                    new { Id = 10, Name = "Лугинський район", RegionId = 1 },
                    new { Id = 11, Name = "Любарський район", RegionId = 1 },
                    new { Id = 12, Name = "Малинський район", RegionId = 1 },
                    new { Id = 13, Name = "Народицький район", RegionId = 1 },
                    new { Id = 14, Name = "Новоград-Волинський район", RegionId = 1 },
                    new { Id = 15, Name = "Овруцький район", RegionId = 1 },
                    new { Id = 16, Name = "Олевський район", RegionId = 1 },
                    new { Id = 17, Name = "Попільнянський район", RegionId = 1 },
                    new { Id = 18, Name = "Радомишльський район", RegionId = 1 },
                    new { Id = 19, Name = "Романівський район", RegionId = 1 },
                    new { Id = 20, Name = "Ружинський район", RegionId = 1 },
                    new { Id = 21, Name = "Пулинський район", RegionId = 1 },
                    new { Id = 22, Name = "Черняхівський район", RegionId = 1 },
                    new { Id = 23, Name = "Чуднівський район", RegionId = 1 }
                    );
            });

            modelBuilder.Entity<Region>(e =>
            {
                e.Property(p => p.Name)
                    .HasMaxLength(100)
                    .HasColumnType("citext")
                    .IsRequired();

                e.HasData(new { Id = 1, Name = "Житомирська обл." });
            });

            modelBuilder.Entity<City>(e =>
            {
                e.Property(p => p.Name)
                    .HasMaxLength(100)
                    .HasColumnType("citext")
                    .IsRequired();

                e.HasOne(p => p.District)
                    .WithMany();

                e.Property(p => p.IsDistrictCity).HasDefaultValue(false);
                e.Property(p => p.IsRegionCity).HasDefaultValue(false);
            });

            modelBuilder.Entity<Street>(e =>
            {
                e.Property(p => p.Name)
                    .HasColumnType("citext")
                    .HasMaxLength(100)
                    .IsRequired();

                e.HasOne(p => p.City)
                    .WithMany();
            });

            modelBuilder.Entity<Address>(e =>
            {
                e.Property(p => p.Building)
                    .HasColumnType("citext")
                    .HasMaxLength(10)
                    .IsRequired();

                e.Property(p => p.Apt)
                    .HasColumnType("citext")
                    .HasMaxLength(5);

                e.HasOne(p => p.Street)
                    .WithMany();
                
                e.HasCheckConstraint("ck_address_zip", "zip ~ '^(\\d){5}$'");
            });

            modelBuilder.Entity<Person>(e =>
            {
                e.Property(p => p.IdCardIssuer)
                    .HasColumnType("citext");

                e.Property(p => p.FirstName)
                    .HasColumnType("citext")
                    .HasMaxLength(50)
                    .IsRequired();

                e.Property(p => p.LastName)
                    .HasMaxLength(50)
                    .HasColumnType("citext")
                    .IsRequired();

                e.Property(p => p.Patronymic)
                    .HasColumnType("citext")
                    .HasMaxLength(50);

                e.Property(p => p.IdCardNumber)
                    .IsRequired()
                    .HasColumnType("citext")
                    .HasMaxLength(9);

                e.Property(p => p.MobilePhones)
                    .HasColumnType("varchar(15)[]");

                e.Property(p => p.TaxCode)
                    .HasMaxLength(10);

                e.HasOne(p => p.Address)
                    .WithMany();

                e.HasIndex(p => p.TaxCode).IsUnique();
                e.HasIndex(p => p.IdCardNumber).IsUnique();
            });

            modelBuilder.Entity<AccountingPoint>(e =>
            {
                e.HasMany(p => p.TariffsHistory)
                    .WithOne()
                    .HasForeignKey(p => p.AccountingPointId);

                e.HasMany(p => p.ContractsHistory)
                    .WithOne()
                    .HasForeignKey(p => p.AccountingPointId);

                e.Property(p => p.Name)
                    .HasColumnType("citext")
                    .HasMaxLength(16)
                    .IsRequired();

                e.Property(p => p.Eic)
                    .HasColumnType("citext")
                    .HasMaxLength(16)
                    .IsRequired();

                e.HasOne(p => p.Owner)
                    .WithMany()
                    .HasForeignKey(p => p.OwnerId);

                e.HasOne(p => p.Address)
                    .WithMany();

                e.HasOne(p => p.DistributionSystemOperator)
                    .WithMany()
                    .HasForeignKey(p => p.DistributionSystemOperatorId);

                e.Property(p => p.Id).HasIdentityOptions(10000000);

                e.HasIndex(p => p.Name).IsUnique();
                e.HasIndex(p => p.Eic).IsUnique();
                e.HasCheckConstraint("CK_accounting_point_eic", "length(eic) = 16");
            });

            modelBuilder.Entity<DistributionSystemOperator>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(200);
                e.HasData(new { Id = 1, Name = "АТ «Житомиробленерго»" }, new { Id = 2, Name = "АТ «Укрзалізниця»" });
            });

            modelBuilder.Entity<Tariff>(e =>
            {
                e.Property(p => p.Name)
                    .HasColumnType("citext")
                    .HasMaxLength(200);
                
                e.HasMany(p => p.Rates).WithOne();
                
                e.HasData(
                     new { Id = 1, Name = "Населення (загальний тариф)" },
                     new { Id = 2, Name = "Будинки з електроопалювальними установками" },
                     new { Id = 3, Name = "Багатоквартирні негазифіковані будинки" },
                     new { Id = 4, Name = "Багатодітні, прийомні сім'ї та дитячі будинки сімейного типу" }
                    );
            });

            modelBuilder.Entity<TariffRate>(e =>
            {
                e.ToTable("tariff_rates")
                    .Property(p => p.Value).HasColumnType("decimal(8,5)");
                
                e.Property(e => e.StartDate).HasColumnType("date");

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
