using Erc.Households.Domain;
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Addresses;
using Erc.Households.Domain.Billing;
using Erc.Households.Domain.Exemptions;
using Erc.Households.Domain.Payments;
using Erc.Households.Domain.Shared;
using Erc.Households.Domain.Shared.Tariffs;
using Erc.Households.Domain.Taxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;

namespace Erc.Households.EF.PostgreSQL
{
    public class ErcContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); });

        public ErcContext(DbContextOptions<ErcContext> options) : base(options)
        {
        }

        static ErcContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Commodity>();
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
        public DbSet<ExemptionCategory> ExemptionCategories { get; set; }
        public DbSet<BuildingType> BuildingTypes { get; set; }
        public DbSet<UsageCategory> UsageCategories { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<TaxInvoice> TaxInvoices { get; set; }
        public DbSet<KFKPayment> KFKPayments { get; set; }
        public DbSet<Company> Company { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention().UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<Commodity>();
            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasMany(bo => bo.BranchOffice)
                    .WithOne(c => c.Company)
                    .HasForeignKey(k => k.CompanyId);

                entity.HasData(
                    new Company
                    {
                        Id = 1, Name = "ТОВАРИСТВО З ОБМЕЖЕНОЮ ВІДПОВІДАЛЬНІСТЮ «ЖИТОМИРСЬКА ОБЛАСНА ЕНЕРГОПОСТАЧАЛЬНА КОМПАНІЯ»", ShortName = "ТОВ «ЖОЕК»", DirectorName = "Гуцало Андрій Анатолійович",
                        Address = "10003, майдан Перемоги, буд. 10 м. Житомир", Email = "kanc@ztoek.com.ua", Www = "https://www.ztoek.com.ua/",
                        TaxpayerPhone = "0412402109", StateRegistryCode = "42095943", TaxpayerNumber = "420959406258", BookkeeperName = "А. В. Івчук", BookkeeperTaxNumber = "2778207983"
                    });
            });

            modelBuilder.Entity<UsageCategory>(entity =>
            {
                entity.Property(e => e.ExemptionDiscountNorms).HasColumnType("jsonb");

                entity.HasData(
                    new
                    {
                        Id = 1,
                        Name = "Звичайне споживання",
                    },
                    new
                    {
                        Id = 2,
                        Name = "Електроплита",
                    },
                    new
                    {
                        Id = 3,
                        Name = "Електроопалювальна установка",
                    },
                    new
                    {
                        Id = 4,
                        Name = "Електроопалювальна установка та електроплита",
                    });

                /* initial data
                  { 4, JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseKWh = 110, BaseKWhWithoutHotWater = 130, BasePerson = 1, KWhPerPerson = 30, MaxKWh = 230, MaxKWhWithoutHotWater = 250, BaseSquareMeter = 10.5m, SquareMeterPerPerson = 21m, KWhPerSquareMeter = 30 } }), "Електроопалювальна установка та електроплита" },
                  { 3, JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseKWh = 70, BaseKWhWithoutHotWater = 100, BasePerson = 1, KWhPerPerson = 30, MaxKWh = 190, MaxKWhWithoutHotWater = 220, BaseSquareMeter = 10.5m, SquareMeterPerPerson = 21m, KWhPerSquareMeter = 30 } }), "Електроопалювальна установка" },
                  { 2, JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseKWh = 110, BaseKWhWithoutHotWater = 130, BasePerson = 1, KWhPerPerson = 30, MaxKWh = 230, MaxKWhWithoutHotWater = 250 } }), "Електроплита" },
                  { 1,  JsonSerializer.Serialize(new[] { new { EffectiveDate = new DateTime(2019, 1, 1), BaseKWh = 70, BaseKWhWithoutHotWater = 100, BasePerson = 1, KWhPerPerson = 30, MaxKWh = 190, MaxKWhWithoutHotWater = 220 } }) , "Звичайне споживання" }
                 */
            });

            modelBuilder.Entity<BuildingType>(entity =>
            {
                entity.HasData(
                    new { Id = 1, Name = "1-2 поверхи", HeataingCorrection = 1.0940m },
                    new { Id = 3, Name = "3 поверхи і більше", HeataingCorrection = 0.7980m }
                    );
            });

            modelBuilder.Entity<ExemptionCategory>(entity =>
            {
                entity.Property(e => e.EffectiveDate).HasColumnType("date");
                entity.Property(e => e.EndDate).HasColumnType("date");
                entity.HasData(
                    new { Id = 1, Name = "Почесні громадяни міста", Coeff = 100.0m, EffectiveDate = new DateTime(2019, 1, 1) },
                    new { Id = 2, Name = "Iнвалiди 1 групи по зору або з ураженням ОРА", Coeff = 50.0m, EffectiveDate = new DateTime(2019, 1, 1), HasLimit = true },
                    new { Id = 3, Name = "Iнвалiди 2 групи по зору або з ураженням ОРА", Coeff = 50.0m, EffectiveDate = new DateTime(2019, 1, 1), HasLimit = true },
                    new { Id = 4, Name = "Учасник бойових дій та членів родин загиблих в АТО (ООС)", Coeff = 100.0m, EffectiveDate = new DateTime(2019, 1, 1), HasLimit = true }
                    );
            });

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
                    new { Id = 202001, StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 1, 31), Name = "Січень 2020р." },
                    new { Id = 202002, StartDate = new DateTime(2020, 2, 1), EndDate = new DateTime(2020, 2, 29), Name = "Лютий 2020р." },
                    new { Id = 202003, StartDate = new DateTime(2020, 3, 1), EndDate = new DateTime(2020, 3, 31), Name = "Березень 2020р." },
                    new { Id = 202004, StartDate = new DateTime(2020, 4, 1), EndDate = new DateTime(2020, 4, 30), Name = "Квітень 2020р." },
                    new { Id = 202005, StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2020, 5, 31), Name = "Травень 2020р." },
                    new { Id = 202006, StartDate = new DateTime(2020, 6, 1), EndDate = new DateTime(2020, 6, 30), Name = "Червень 2020р." },
                    new { Id = 202007, StartDate = new DateTime(2020, 7, 1), EndDate = new DateTime(2020, 7, 31), Name = "Липень 2020р." },
                    new { Id = 202008, StartDate = new DateTime(2020, 8, 1), EndDate = new DateTime(2020, 8, 31), Name = "Серпень 2020р." },
                    new { Id = 202009, StartDate = new DateTime(2020, 9, 1), EndDate = new DateTime(2020, 9, 30), Name = "Вересень 2020р." }
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
                entity.Property(p => p.TotalCharge).HasColumnType("decimal(10,2)");

                entity.Property(e => e.FromDate).HasColumnType("date");
                entity.Property(e => e.ToDate).HasColumnType("date");
                entity.Property(b => b.UsageT1).HasColumnType("jsonb");
                entity.Property(b => b.UsageT2).HasColumnType("jsonb");
                entity.Property(b => b.UsageT3).HasColumnType("jsonb");

                entity.HasOne(e => e.Tariff).WithMany();
                entity.HasIndex(p => p.DsoConsumptionId).IsUnique();
            });

            //modelBuilder.Entity<Contract>(e =>
            //{
            //    e.ToTable("contracts")
            //        .Property(p => p.Logs).HasColumnType("jsonb");

            //    e.Property(e => e.StartDate).HasColumnType("date");
            //    e.Property(e => e.EndDate).HasColumnType("date");
            //});

            //modelBuilder.Entity<AccountingPointTariff>(e =>
            //{
            //    e.ToTable("accounting_point_tariffs");

            //    e.Property(e => e.StartDate).HasColumnType("date");

            //    e.Property(p => p.Logs).HasColumnType("jsonb");

            //    e.HasOne(p => p.Tariff)
            //        .WithMany()
            //        .HasForeignKey(p => p.TariffId);
            //});

            modelBuilder.Entity<AccountingPointDebtHistory>(entity =>
            {
                entity.HasKey(e => new { e.AccountingPointId, e.PeriodId });
                entity.Property(p => p.DebtValue).HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<BranchOffice>(entity =>
            {
                entity.Property(p => p.StringId)
                    .HasColumnType("citext")
                    .HasMaxLength(2).IsRequired();

                entity.Property(p => p.Address)
                    .HasColumnType("citext")
                    .HasMaxLength(500).IsRequired();

                entity.Property(p => p.Name)
                    .HasColumnType("citext")
                    .HasMaxLength(200).IsRequired();
                entity.HasData(
                    new { Id = 1, CurrentPeriodId = 201901, Name = "Андрушівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "an", DistrictIds = new[] { 1 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 2, CurrentPeriodId = 201901, Name = "Баранiвський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "bn", DistrictIds = new[] { 2 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 3, CurrentPeriodId = 201901, Name = "Бердичiвський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "bd", DistrictIds = new[] { 3 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 4, CurrentPeriodId = 201901, Name = "Брусилівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "br", DistrictIds = new[] { 4 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 5, CurrentPeriodId = 201901, Name = "Хорошівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "hr", DistrictIds = new[] { 5 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 6, CurrentPeriodId = 201901, Name = "Ємільчинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "em", DistrictIds = new[] { 6 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 7, CurrentPeriodId = 201901, Name = "Житомирський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "zt", DistrictIds = new[] { 7 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 8, CurrentPeriodId = 201901, Name = "Зарічанський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "zr", DistrictIds = new[] { 7 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 9, CurrentPeriodId = 201901, Name = "Коростенський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "kr", DistrictIds = new[] { 8, 10 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 10, CurrentPeriodId = 201901, Name = "Коростишiвський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "kt", DistrictIds = new[] { 9 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 11, CurrentPeriodId = 201901, Name = "Любарський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "lb", DistrictIds = new[] { 11 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 12, CurrentPeriodId = 201901, Name = "Малинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ml", DistrictIds = new[] { 12 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 13, CurrentPeriodId = 201901, Name = "Народицький ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "nr", DistrictIds = new[] { 13 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 14, CurrentPeriodId = 201901, Name = "Новоград-Волинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "nv", DistrictIds = new[] { 14 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 15, CurrentPeriodId = 201901, Name = "Овруцький ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ov", DistrictIds = new[] { 15 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 16, CurrentPeriodId = 201901, Name = "Олевський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ol", DistrictIds = new[] { 16 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 17, CurrentPeriodId = 201901, Name = "Попільнянський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "pp", DistrictIds = new[] { 17, 20 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 18, CurrentPeriodId = 201901, Name = "Радомишльський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "rd", DistrictIds = new[] { 18 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 19, CurrentPeriodId = 201901, Name = "Романівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "rm", DistrictIds = new[] { 19 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 20, CurrentPeriodId = 201901, Name = "Пулинський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "pl", DistrictIds = new[] { 21 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 21, CurrentPeriodId = 201901, Name = "Черняхівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "ch", DistrictIds = new[] { 22 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 22, CurrentPeriodId = 201901, Name = "Чуднівський ЦОК", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "cd", DistrictIds = new[] { 23 }, AvailableCommodities = new[] { Commodity.ElectricPower }, CompanyId = 1 },
                    new { Id = 101, CurrentPeriodId = 201901, Name = "Центральний офіс", Address = "10003, м. Житомир, майдан Перемоги, 10", StringId = "co", DistrictIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }, AvailableCommodities = new[] { Commodity.NaturalGas }, CompanyId = 1 }
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
                
                //e.HasCheckConstraint("ck_address_zip", "zip ~ '^(\\d){5}$'");
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

            modelBuilder.Entity<AccountingPoint>(entity =>
            {
                entity.Property(e => e.Commodity).HasDefaultValue(Commodity.ElectricPower);

                entity.OwnsMany(p => p.Exemptions, r =>
                {
                    r.ToTable("accounting_point_exemptions")
                        .WithOwner()
                        .HasForeignKey(e => e.AccountingPointId);
                    
                    r.Property(e => e.EffectiveDate).HasColumnType("date");
                    
                    r.Property(e => e.EndDate).HasColumnType("date");
                    
                    r.Property(e => e.HasLimit).HasDefaultValue(true);
                });
                
                entity.Property(p => p.Debt).HasColumnType("decimal(10,2)");

                entity.OwnsMany(p => p.TariffsHistory, t =>
                {
                    t.ToTable("accounting_point_tariffs").WithOwner().HasForeignKey(p => p.AccountingPointId);
                    t.Property(p => p.Logs).HasColumnType("jsonb");
                    t.Property(p => p.StartDate).HasColumnType("date");
                });

                entity.OwnsMany(p => p.ContractsHistory, t =>
                {
                    t.ToTable("contracts").WithOwner().HasForeignKey(p => p.AccountingPointId);
                    t.Property(p => p.Logs).HasColumnType("jsonb");
                    t.Property(p => p.StartDate).HasColumnType("date");
                    t.Property(p => p.EndDate).HasColumnType("date");
                    t.Property(v => v.SendPaperBill).HasDefaultValue(true);
                });

                entity.Property(p => p.Name)
                    .HasColumnType("citext")
                    .HasMaxLength(16)
                    .IsRequired();

                entity.Property(p => p.Eic)
                    .HasColumnType("citext")
                    .HasMaxLength(16)
                    .IsRequired();

                entity.HasOne(p => p.Owner)
                    .WithMany()
                    .HasForeignKey(p => p.OwnerId);

                entity.HasOne(p => p.Address)
                    .WithMany();

                entity.HasOne(p => p.DistributionSystemOperator)
                    .WithMany()
                    .HasForeignKey(p => p.DistributionSystemOperatorId);

                entity.Property(p => p.Id).HasIdentityOptions(10000000);

                entity.HasIndex(p => p.Name).IsUnique();
                entity.HasIndex(p => p.Eic).IsUnique();
                entity.HasCheckConstraint("CK_accounting_point_eic", "length(eic) = 16");
            });

            modelBuilder.Entity<DistributionSystemOperator>(entity =>
            {
                entity.Property(p => p.Name).HasMaxLength(200);
                entity.HasData(
                    new { Id = 1, Name = "АТ «Житомиробленерго»", Commodity = Commodity.ElectricPower }, 
                    new { Id = 2, Name = "АТ «Укрзалізниця»", Commodity = Commodity.ElectricPower },
                    new { Id = 101, Name = "АТ «ЖИТОМИРГАЗ»", Commodity = Commodity.NaturalGas }
                    );
            });

            modelBuilder.Entity<Tariff>(entity =>
            {
                entity.Property(p => p.Name)
                    .HasColumnType("citext")
                    .HasMaxLength(200);

                entity.Property(e => e.Rates).HasColumnType("jsonb");

                entity.HasData(
                     new
                     {
                         Id = 1,
                         Name = "Населення (загальний тариф)",
                         Commodity = Commodity.ElectricPower,
                     },

                     new
                     {
                         Id = 2,
                         Name = "Будинки з електроопалювальними установками",
                         Commodity = Commodity.ElectricPower,
                     },

                     new
                     {
                         Id = 3,
                         Name = "Багатоквартирні негазифіковані будинки",
                         Commodity = Commodity.ElectricPower,
                     },

                     new
                     {
                         Id = 4,
                         Name = "Багатодітні, прийомні сім'ї та дитячі будинки сімейного типу",
                         Commodity = Commodity.ElectricPower,
                     },

                     new { Id = 101, Name = "Природний газ для населення", Commodity = Commodity.NaturalGas }
                    );
                // init data
                //{
                //    {
                //        3, Commodity.ElectricPower, "Багатоквартирні негазифіковані будинки",
                //        JsonSerializer.Serialize(new[]
                //         {
                //            new TariffRate { Id = 1, StartDate = new DateTime(2019, 1, 1), Value = 0.90m, ConsumptionLimit = 100, HeatingConsumptionLimit = 3000, HeatingStartDay = new DateTime(2019, 10, 01), HeatingEndDay = new DateTime(2020, 04, 30)},
                //            new TariffRate { Id = 2, StartDate = new DateTime(2019, 1, 1), Value = 1.68m, }
                //         })
                //    },
                //    {
                //        2, Commodity.ElectricPower, "Будинки з електроопалювальними установками",
                //        JsonSerializer.Serialize(new[]
                //         {
                //            new TariffRate { Id = 1, StartDate = new DateTime(2019, 1, 1), Value = 0.90m, ConsumptionLimit = 100, HeatingConsumptionLimit = 3000, HeatingStartDay = new DateTime(2019, 10, 01), HeatingEndDay = new DateTime(2020, 04, 30) },
                //            new TariffRate { Id = 2, StartDate = new DateTime(2019, 1, 1), Value = 1.68m}
                //         })
                //    },
                //    { 1, Commodity.ElectricPower, "Населення (загальний тариф)",  JsonSerializer.Serialize(new[] { new TariffRate { Id = 1, StartDate = new DateTime(2019, 1, 1), Value = 0.9m, ConsumptionLimit = 100 }, new TariffRate { StartDate = new DateTime(2019, 1, 1), Value = 1.68m } })},
                //    { 101, Commodity.NaturalGas, "Природний газ для населення", null },
                //    { 4, Commodity.ElectricPower, "Багатодітні, прийомні сім'ї та дитячі будинки сімейного типу", JsonSerializer.Serialize(new[] { new TariffRate { Id = 1, StartDate = new DateTime(2019, 1, 1), Value = 1.68m } }) }
                //}
            });

            modelBuilder.Entity<TaxInvoice>(entity =>
            {
                entity.Property(p => p.Id)
                    .HasIdentityOptions(300000);

                entity.Property(p => p.LiabilitySum).HasColumnType("decimal(19,2)");
                entity.Property(p => p.TariffValue).HasColumnType("decimal(9,8)");
                entity.Property(p => p.TaxSum).HasColumnType("decimal(19,6)");
                entity.Property(p => p.FullSum).HasColumnType("decimal(24,6)");

                entity.HasOne(p => p.BranchOffice)
                    .WithMany()
                    .HasForeignKey(p => p.BranchOfficeId);
            });

            modelBuilder.Entity<KFKPayment>(entity =>
            {
                entity.HasOne(p => p.Period)
                    .WithMany()
                    .HasForeignKey(p => p.PeriodId);
            });
        }
    }
}
