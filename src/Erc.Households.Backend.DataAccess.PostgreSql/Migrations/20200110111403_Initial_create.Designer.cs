﻿// <auto-generated />
using System;
using Erc.Households.Backend.DataAccess.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.Backend.DataAccess.PostgreSql.Migrations
{
    [DbContext(typeof(ErcContext))]
    [Migration("20200110111403_Initial_create")]
    partial class Initial_create
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Erc.Households.Backend.Data.AccountingPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:IdentitySequenceOptions", "'10000000', '1', '', '', 'False', '1'")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AddressId")
                        .HasColumnName("address_id")
                        .HasColumnType("integer");

                    b.Property<int>("DistributionSystemOperatorId")
                        .HasColumnName("distribution_system_operator_id")
                        .HasColumnType("integer");

                    b.Property<string>("Eic")
                        .IsRequired()
                        .HasColumnName("eic")
                        .HasColumnType("character varying(16)")
                        .HasMaxLength(16);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("character varying(16)")
                        .HasMaxLength(16);

                    b.Property<int>("OwnerId")
                        .HasColumnName("owner_id")
                        .HasColumnType("integer");

                    b.Property<int>("TariffId")
                        .HasColumnName("tariff_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("DistributionSystemOperatorId");

                    b.HasIndex("Eic")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.HasIndex("TariffId");

                    b.ToTable("accounting_points");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Addresses.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Apt")
                        .HasColumnName("apt")
                        .HasColumnType("character varying(5)")
                        .HasMaxLength(5);

                    b.Property<string>("Building")
                        .IsRequired()
                        .HasColumnName("building")
                        .HasColumnType("character varying(20)")
                        .HasMaxLength(20);

                    b.Property<int>("StreetId")
                        .HasColumnName("street_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StreetId");

                    b.ToTable("address");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Addresses.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("DistrictId")
                        .HasColumnName("district_id")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDistrictCity")
                        .HasColumnName("is_district_city")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRegionCity")
                        .HasColumnName("is_region_city")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("city");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Addresses.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<int>("RegionId")
                        .HasColumnName("region_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("districts");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Addresses.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("regions");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Addresses.Street", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CityId")
                        .HasColumnName("city_id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("street");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.BranchOffice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnName("address")
                        .HasColumnType("character varying(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.Property<string>("StringId")
                        .IsRequired()
                        .HasColumnName("string_id")
                        .HasColumnType("character varying(2)")
                        .HasMaxLength(2);

                    b.HasKey("Id");

                    b.ToTable("branch_offices");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.DistributionSystemOperator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("distribution_system_operators");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AddressId")
                        .HasColumnName("address_id")
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("first_name")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("IdCardExpDate")
                        .HasColumnName("id_card_exp_date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("IdCardNumber")
                        .HasColumnName("id_card_number")
                        .HasColumnType("character varying(9)")
                        .HasMaxLength(9);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("last_name")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("MobilePhone1")
                        .HasColumnName("mobile_phone1")
                        .HasColumnType("character varying(15)")
                        .HasMaxLength(15);

                    b.Property<string>("MobilePhone2")
                        .HasColumnName("mobile_phone2")
                        .HasColumnType("character varying(15)")
                        .HasMaxLength(15);

                    b.Property<string>("Patronymic")
                        .HasColumnName("patronymic")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("TaxCode")
                        .HasColumnName("tax_code")
                        .HasColumnType("character varying(10)")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("IdCardNumber")
                        .IsUnique();

                    b.HasIndex("TaxCode")
                        .IsUnique();

                    b.ToTable("person");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Tariffs.Tariff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("tariffs");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Tariffs.TariffRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ConsumptionLimit")
                        .HasColumnName("consumption_limit")
                        .HasColumnType("integer");

                    b.Property<int?>("HeatingConsumptionLimit")
                        .HasColumnName("heating_consumption_limit")
                        .HasColumnType("integer");

                    b.Property<int?>("HeatingEndDay")
                        .HasColumnName("heating_end_day")
                        .HasColumnType("integer");

                    b.Property<int?>("HeatingEndMonth")
                        .HasColumnName("heating_end_month")
                        .HasColumnType("integer");

                    b.Property<int?>("HeatingStartDay")
                        .HasColumnName("heating_start_day")
                        .HasColumnType("integer");

                    b.Property<int?>("HeatingStartMonth")
                        .HasColumnName("heating_start_month")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("start_date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("TariffId")
                        .HasColumnName("tariff_id")
                        .HasColumnType("integer");

                    b.Property<decimal>("Value")
                        .HasColumnName("value")
                        .HasColumnType("decimal(8,5)");

                    b.HasKey("Id");

                    b.HasIndex("TariffId");

                    b.ToTable("tariff_rates");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.AccountingPoint", b =>
                {
                    b.HasOne("Erc.Households.Backend.Data.Addresses.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Erc.Households.Backend.Data.DistributionSystemOperator", "Dso")
                        .WithMany()
                        .HasForeignKey("DistributionSystemOperatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Erc.Households.Backend.Data.Person", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Erc.Households.Backend.Data.Tariffs.Tariff", "Tariff")
                        .WithMany()
                        .HasForeignKey("TariffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Addresses.Address", b =>
                {
                    b.HasOne("Erc.Households.Backend.Data.Addresses.Street", "Street")
                        .WithMany()
                        .HasForeignKey("StreetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Addresses.City", b =>
                {
                    b.HasOne("Erc.Households.Backend.Data.Addresses.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Addresses.District", b =>
                {
                    b.HasOne("Erc.Households.Backend.Data.Addresses.Region", "Region")
                        .WithMany("Districts")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Addresses.Street", b =>
                {
                    b.HasOne("Erc.Households.Backend.Data.Addresses.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Person", b =>
                {
                    b.HasOne("Erc.Households.Backend.Data.Addresses.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");
                });

            modelBuilder.Entity("Erc.Households.Backend.Data.Tariffs.TariffRate", b =>
                {
                    b.HasOne("Erc.Households.Backend.Data.Tariffs.Tariff", null)
                        .WithMany("Rates")
                        .HasForeignKey("TariffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}