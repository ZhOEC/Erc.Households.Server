using System;
using System.Collections.Generic;
using Erc.Households.Server.ModelLogs;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Erc.Households.Server.DataAccess.EF.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "branch_offices",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 200, nullable: false),
                    string_id = table.Column<string>(type: "citext", maxLength: 2, nullable: false),
                    district_ids = table.Column<int[]>(nullable: true),
                    address = table.Column<string>(type: "citext", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_branch_offices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "distribution_system_operators",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_distribution_system_operators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tariffs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tariffs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    region_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_districts", x => x.id);
                    table.ForeignKey(
                        name: "fk_districts_regions_region_id",
                        column: x => x.region_id,
                        principalTable: "regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tariff_rates",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_date = table.Column<DateTime>(nullable: false),
                    value = table.Column<decimal>(type: "decimal(8,5)", nullable: false),
                    consumption_limit = table.Column<int>(nullable: true),
                    heating_consumption_limit = table.Column<int>(nullable: true),
                    heating_start_day = table.Column<DateTime>(nullable: true),
                    heating_end_day = table.Column<DateTime>(nullable: true),
                    tariff_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tariff_rate", x => x.id);
                    table.ForeignKey(
                        name: "fk_tariff_rate_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    district_id = table.Column<int>(nullable: false),
                    is_district_city = table.Column<bool>(nullable: false, defaultValue: false),
                    is_region_city = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cities", x => x.id);
                    table.ForeignKey(
                        name: "fk_cities_districts_district_id",
                        column: x => x.district_id,
                        principalTable: "districts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "streets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                    city_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_streets", x => x.id);
                    table.ForeignKey(
                        name: "fk_streets_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    zip = table.Column<string>(nullable: true),
                    street_id = table.Column<int>(nullable: false),
                    building = table.Column<string>(type: "citext", maxLength: 10, nullable: false),
                    apt = table.Column<string>(type: "citext", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_addresses", x => x.id);
                    table.CheckConstraint("ck_address_zip", "zip ~ '^(\\d){5}$'");
                    table.ForeignKey(
                        name: "fk_addresses_streets_street_id",
                        column: x => x.street_id,
                        principalTable: "streets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "citext", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "citext", maxLength: 50, nullable: false),
                    patronymic = table.Column<string>(type: "citext", maxLength: 50, nullable: true),
                    tax_code = table.Column<string>(maxLength: 10, nullable: true),
                    id_card_number = table.Column<string>(type: "citext", maxLength: 9, nullable: false),
                    id_card_issuance_date = table.Column<DateTime>(nullable: false),
                    id_card_exp_date = table.Column<DateTime>(nullable: true),
                    address_id = table.Column<int>(nullable: true),
                    mobile_phones = table.Column<string[]>(type: "varchar(15)[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_people", x => x.id);
                    table.ForeignKey(
                        name: "fk_people_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "accounting_points",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'10000000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "citext", maxLength: 16, nullable: false),
                    eic = table.Column<string>(type: "citext", maxLength: 16, nullable: false),
                    address_id = table.Column<int>(nullable: false),
                    owner_id = table.Column<int>(nullable: false),
                    distribution_system_operator_id = table.Column<int>(nullable: false),
                    branch_office_id = table.Column<int>(nullable: false),
                    debt = table.Column<decimal>(nullable: false),
                    contract_is_signed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounting_points", x => x.id);
                    table.CheckConstraint("CK_accounting_point_eic", "length(eic) = 16");
                    table.ForeignKey(
                        name: "fk_accounting_points_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_points_branch_offices_branch_office_id",
                        column: x => x.branch_office_id,
                        principalTable: "branch_offices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_points_distribution_system_operators_distributio",
                        column: x => x.distribution_system_operator_id,
                        principalTable: "distribution_system_operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_points_people_owner_id",
                        column: x => x.owner_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accounting_point_tariffs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    logs = table.Column<IReadOnlyCollection<ObjectLog>>(type: "jsonb", nullable: true),
                    start_date = table.Column<DateTime>(nullable: false),
                    accounting_point_id = table.Column<int>(nullable: false),
                    tariff_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounting_point_tariff", x => x.id);
                    table.ForeignKey(
                        name: "fk_accounting_point_tariff_accounting_points_accounting_point_id",
                        column: x => x.accounting_point_id,
                        principalTable: "accounting_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accounting_point_tariff_tariffs_tariff_id",
                        column: x => x.tariff_id,
                        principalTable: "tariffs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contracts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    logs = table.Column<IReadOnlyCollection<ObjectLog>>(type: "jsonb", nullable: true),
                    accounting_point_id = table.Column<int>(nullable: false),
                    customer_id = table.Column<int>(nullable: false),
                    start_date = table.Column<DateTime>(nullable: false),
                    end_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contract", x => x.id);
                    table.ForeignKey(
                        name: "fk_contract_accounting_points_accounting_point_id",
                        column: x => x.accounting_point_id,
                        principalTable: "accounting_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_contract_people_customer_id",
                        column: x => x.customer_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "branch_offices",
                columns: new[] { "id", "address", "district_ids", "name", "string_id" },
                values: new object[,]
                {
                    { 14, "10003, м. Житомир, майдан Перемоги, 10", new[] { 14 }, "Новоград-Волинський ЦОК", "nv" },
                    { 21, "10003, м. Житомир, майдан Перемоги, 10", new[] { 22 }, "Черняхівський ЦОК", "ch" },
                    { 20, "10003, м. Житомир, майдан Перемоги, 10", new[] { 21 }, "Пулинський ЦОК", "pl" },
                    { 19, "10003, м. Житомир, майдан Перемоги, 10", new[] { 19 }, "Романівський ЦОК", "rm" },
                    { 18, "10003, м. Житомир, майдан Перемоги, 10", new[] { 18 }, "Радомишльський ЦОК", "rd" },
                    { 17, "10003, м. Житомир, майдан Перемоги, 10", new[] { 17, 20 }, "Попільнянський ЦОК", "pp" },
                    { 16, "10003, м. Житомир, майдан Перемоги, 10", new[] { 16 }, "Олевський ЦОК", "ol" },
                    { 15, "10003, м. Житомир, майдан Перемоги, 10", new[] { 15 }, "Овруцький ЦОК", "ov" },
                    { 13, "10003, м. Житомир, майдан Перемоги, 10", new[] { 13 }, "Народицький ЦОК", "nr" },
                    { 12, "10003, м. Житомир, майдан Перемоги, 10", new[] { 12 }, "Малинський ЦОК", "ml" },
                    { 22, "10003, м. Житомир, майдан Перемоги, 10", new[] { 23 }, "Чуднівський ЦОК", "cd" },
                    { 11, "10003, м. Житомир, майдан Перемоги, 10", new[] { 11 }, "Любарський ЦОК", "lb" },
                    { 9, "10003, м. Житомир, майдан Перемоги, 10", new[] { 8, 10 }, "Коростенський ЦОК", "kr" },
                    { 8, "10003, м. Житомир, майдан Перемоги, 10", new[] { 7 }, "Зарічанський ЦОК", "zr" },
                    { 7, "10003, м. Житомир, майдан Перемоги, 10", new[] { 7 }, "Житомирський ЦОК", "zt" },
                    { 6, "10003, м. Житомир, майдан Перемоги, 10", new[] { 6 }, "Ємільчинський ЦОК", "em" },
                    { 5, "10003, м. Житомир, майдан Перемоги, 10", new[] { 5 }, "Хорошівський ЦОК", "hr" },
                    { 4, "10003, м. Житомир, майдан Перемоги, 10", new[] { 4 }, "Брусилівський ЦОК", "br" },
                    { 3, "10003, м. Житомир, майдан Перемоги, 10", new[] { 3 }, "Бердичiвський ЦОК", "bd" },
                    { 2, "10003, м. Житомир, майдан Перемоги, 10", new[] { 2 }, "Баранiвський ЦОК", "bn" },
                    { 1, "10003, м. Житомир, майдан Перемоги, 10", new[] { 1 }, "Андрушівський ЦОК", "an" },
                    { 10, "10003, м. Житомир, майдан Перемоги, 10", new[] { 9 }, "Коростишiвський ЦОК", "kt" }
                });

            migrationBuilder.InsertData(
                table: "distribution_system_operators",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 2, "АТ «Укрзалізниця»" },
                    { 1, "АТ «Житомиробленерго»" }
                });

            migrationBuilder.InsertData(
                table: "regions",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "Житомирська обл." });

            migrationBuilder.InsertData(
                table: "tariffs",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Населення (загальний тариф)" },
                    { 2, "Будинки з електроопалювальними установками" },
                    { 3, "Багатоквартирні негазифіковані будинки" },
                    { 4, "Багатодітні, прийомні сім''ї та дитячі будинки сімейного типу" }
                });

            migrationBuilder.InsertData(
                table: "districts",
                columns: new[] { "id", "name", "region_id" },
                values: new object[,]
                {
                    { 1, "Андрушівський район", 1 },
                    { 23, "Чуднівський район", 1 },
                    { 22, "Черняхівський район", 1 },
                    { 21, "Пулинський район", 1 },
                    { 20, "Ружинський район", 1 },
                    { 19, "Романівський район", 1 },
                    { 18, "Радомишльський район", 1 },
                    { 17, "Попільнянський район", 1 },
                    { 16, "Олевський район", 1 },
                    { 14, "Новоград-Волинський район", 1 },
                    { 13, "Народицький район", 1 },
                    { 15, "Овруцький район", 1 },
                    { 11, "Любарський район", 1 },
                    { 2, "Баранiвський район", 1 },
                    { 3, "Бердичiвський район", 1 },
                    { 12, "Малинський район", 1 },
                    { 5, "Хорошівський район", 1 },
                    { 6, "Ємільчинський район", 1 },
                    { 4, "Брусилівський район", 1 },
                    { 8, "Коростенський район", 1 },
                    { 9, "Коростишiвський район", 1 },
                    { 10, "Лугинський район", 1 },
                    { 7, "Житомирський район", 1 }
                });

            migrationBuilder.InsertData(
                table: "tariff_rates",
                columns: new[] { "id", "consumption_limit", "heating_consumption_limit", "heating_end_day", "heating_start_day", "start_date", "tariff_id", "value" },
                values: new object[,]
                {
                    { 6, null, null, null, null, new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1.68m },
                    { 1, 100, null, null, null, new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0.9m },
                    { 2, null, null, null, null, new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1.68m },
                    { 3, 100, 3000, new DateTime(2020, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 0.90m },
                    { 4, null, null, null, null, new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1.68m },
                    { 5, 100, 3000, new DateTime(2020, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 0.90m },
                    { 7, null, null, null, null, new DateTime(2017, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 0.90m }
                });

            migrationBuilder.CreateIndex(
                name: "ix_accounting_point_tariff_accounting_point_id",
                table: "accounting_point_tariffs",
                column: "accounting_point_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_point_tariff_tariff_id",
                table: "accounting_point_tariffs",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_address_id",
                table: "accounting_points",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_branch_office_id",
                table: "accounting_points",
                column: "branch_office_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_distribution_system_operator_id",
                table: "accounting_points",
                column: "distribution_system_operator_id");

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_eic",
                table: "accounting_points",
                column: "eic",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_name",
                table: "accounting_points",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_accounting_points_owner_id",
                table: "accounting_points",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_addresses_street_id",
                table: "addresses",
                column: "street_id");

            migrationBuilder.CreateIndex(
                name: "ix_cities_district_id",
                table: "cities",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "ix_contract_accounting_point_id",
                table: "contracts",
                column: "accounting_point_id");

            migrationBuilder.CreateIndex(
                name: "ix_contract_customer_id",
                table: "contracts",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_districts_region_id",
                table: "districts",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_people_address_id",
                table: "people",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_people_id_card_number",
                table: "people",
                column: "id_card_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_people_tax_code",
                table: "people",
                column: "tax_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_streets_city_id",
                table: "streets",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_tariff_rate_tariff_id",
                table: "tariff_rates",
                column: "tariff_id");

            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX uix_addresses_street_id_building_apt ON public.addresses 
                USING btree (street_id, building, coalesce(apt,'') ASC NULLS LAST)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounting_point_tariffs");

            migrationBuilder.DropTable(
                name: "contracts");

            migrationBuilder.DropTable(
                name: "tariff_rates");

            migrationBuilder.DropTable(
                name: "accounting_points");

            migrationBuilder.DropTable(
                name: "tariffs");

            migrationBuilder.DropTable(
                name: "branch_offices");

            migrationBuilder.DropTable(
                name: "distribution_system_operators");

            migrationBuilder.DropTable(
                name: "people");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "streets");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "regions");
        }
    }
}
